using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_Repuestos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _correo;

        public AutoController(AppDbContext context, IEmailService correo)
        {
            _context = context;
            _correo = correo;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOregistro request)
        {

            // Generar código de verificación
            var codigo = new Random().Next(100000, 999999).ToString();

            // Validar modelo
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errores });
            }

            // Validar duplicados en usuarios reales
            if (await _context.Usuarios.AnyAsync(u => u.Correo == request.Correo))
                return BadRequest(new { mensaje = "El correo ya está registrado, ingresa uno diferente." });

            if (await _context.Usuarios.AnyAsync(u => u.user == request.User))
                return BadRequest(new { mensaje = "El usuario ya está registrado, ingresa uno diferente." });

            if (await _context.Usuarios.AnyAsync(u => u.DNI == request.DNI))
                return BadRequest(new { mensaje = "El DNI ya está registrado, ingresa uno diferente." });

            // Ver si ya había un registro temporal pendiente con ese correo
            var existenteTemp = await _context.UsuariosTemporales
                .FirstOrDefaultAsync(u => u.Correo == request.Correo);

            if (existenteTemp != null)
            {
                _context.UsuariosTemporales.Remove(existenteTemp);
                await _context.SaveChangesAsync();
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Contrasenia);

            // Crear registro temporal
            var temp = new UsuarioTemp
            {
                Nombre_apellido = request.Nombre_apellido,
                DNI = request.DNI,
                Direccion = request.Direccion,
                Correo = request.Correo,
                User = request.User,
                Contrasenia = hash,
                Rol = request.Rol,
                CodigoVerificacion = codigo,
                FechaExpira = DateTime.Now.AddMinutes(10)
            };

            _context.UsuariosTemporales.Add(temp);
            Console.WriteLine($"[DEBUG] Guardando temporal: {temp.User} - {temp.Correo}");
            await _context.SaveChangesAsync();

            // Enviar código al correo
            await _correo.EnviarCodigoAsync(request.Correo, codigo);

            return Ok(new
            {
                mensaje = "Se envió un código de verificación al correo. Confirma para crear tu cuenta antes de 10 minutos."
            });

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.user == request.User);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, usuario.Contrasenia))
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
             
            // Generar OTP
            var otp = Random.Shared.Next(100000, 999999).ToString();
            usuario.CodigoVerificacion = otp;
            usuario.CodigoExpira = DateTime.UtcNow.AddMinutes(5);
            await _context.SaveChangesAsync();

            // Enviar por correo
            await _correo.EnviarCodigoAsync(usuario.Correo, otp);

            return Ok(new
            {
                requiereCodigo = true,
                mensaje = "Se envió un código de verificación a tu correo. Valídalo antes de continuar."
            });
        }
    


    [HttpPost("verificar-codigo")]
        public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoDTO request, [FromServices] IConfiguration cfg)
        {
            // 🔹 Primero verificamos si es un usuario real (2FA)
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.user == request.User);

            if (usuario != null)
            {
                // Es un usuario existente → verificar su 2FA
                if (usuario.CodigoVerificacion != request.Codigo)
                    return Unauthorized(new { mensaje = "Código inválido." });

                if (usuario.CodigoExpira < DateTime.UtcNow)
                {
                    usuario.CodigoVerificacion = null;
                    usuario.CodigoExpira = null;
                    await _context.SaveChangesAsync();
                    return Unauthorized(new { mensaje = "El código expiró, solicita uno nuevo." });
                }

                // ✅ Código válido → limpiar y generar token
                usuario.CodigoVerificacion = null;
                usuario.CodigoExpira = null;
                await _context.SaveChangesAsync();

                var token = GenerarJwt(usuario, cfg);

                return Ok(new
                {
                    mensaje = "Login exitoso (2FA confirmado)",
                    usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.user,
                        usuario.rol,
                        usuario.Correo,
                        usuario.Nombre_apellido,
                        usuario.DNI,
                        usuario.Direccion
                    },
                    token
                });
            }

            // Si no existe en Usuarios, buscamos en UsuariosTemporales
            var temporal = await _context.UsuariosTemporales.FirstOrDefaultAsync(u => u.User == request.User);

            if (temporal == null)
                return Unauthorized(new { mensaje = "Usuario inválido o sin registro temporal." });

            // Verificar código
            if (temporal.CodigoVerificacion != request.Codigo)
                return Unauthorized(new { mensaje = "Código inválido." });

            if (temporal.FechaExpira < DateTime.Now)
            {
                Console.WriteLine($"[DEBUG] Código expirado para usuario temporal {temporal.User}. FechaExpira={temporal.FechaExpira}, Ahora={DateTime.Now}");
                return Unauthorized(new { mensaje = "El código expiró. Regístrate nuevamente." });
            }

            // Si el código es correcto → crear usuario real
            var nuevoUsuario = new Usuario
            {
                Nombre_apellido = temporal.Nombre_apellido,
                DNI = temporal.DNI,
                Direccion = temporal.Direccion,
                Correo = temporal.Correo,
                user = temporal.User,
                Contrasenia = temporal.Contrasenia,
                rol = temporal.Rol
            };

            _context.Usuarios.Add(nuevoUsuario);

            // Borrar solo ahora el temporal (después de crear el real)
            _context.UsuariosTemporales.Remove(temporal);
            await _context.SaveChangesAsync();

            var tokenNuevo = GenerarJwt(nuevoUsuario, cfg);

            return Ok(new
            {
                mensaje = "Cuenta verificada y creada exitosamente.",
                usuario = new
                {
                    nuevoUsuario.IdUsuario,
                    nuevoUsuario.user,
                    nuevoUsuario.rol,
                    nuevoUsuario.Correo,
                    nuevoUsuario.Nombre_apellido,
                    nuevoUsuario.DNI,
                    nuevoUsuario.Direccion
                },
                token = tokenNuevo
            });
        }

        
        private string GenerarJwt(Usuario usuario, IConfiguration cfg)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
        new Claim("user", usuario.user),
        new Claim("rol", usuario.rol)
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: cfg["Jwt:Issuer"],
                audience: cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}



