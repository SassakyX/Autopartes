using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOregistro request)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Devolvemos errores en un JSON que Angular pueda mostrar
                return BadRequest(new { errores });
            }

            if (await _context.Usuarios.AnyAsync(u => u.user == request.User || u.Correo == request.Correo))
            {
                return BadRequest(new { mensaje = "El usuario o correo ya está registrado." });
            }

            var usuario = new Usuario
            {
                Nombre_apellido = request.Nombre_apellido,
                DNI = request.DNI,
                Direccion = request.Direccion,
                Correo = request.Correo,
                user = request.User,
                Contrasenia = BCrypt.Net.BCrypt.HashPassword(request.Contrasenia),
                rol = "Usuario"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado correctamente" });
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

            return Ok(new { mensaje = "Se envió un código de verificación a tu correo" });
        }
    


    [HttpPost("verificar-codigo")]
        public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoDTO request, [FromServices] IConfiguration cfg)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.user == request.User);
            if (usuario == null) return Unauthorized(new { mensaje = "Usuario inválido" });

            if (usuario.CodigoVerificacion != request.Codigo || usuario.CodigoExpira < DateTime.UtcNow)
                return Unauthorized(new { mensaje = "Código inválido o expirado" });

            // limpiar OTP
            usuario.CodigoVerificacion = null;
            usuario.CodigoExpira = null;
            await _context.SaveChangesAsync();

            // generar JWT
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

            return Ok(new
            {
                mensaje = "Login exitoso",
                usuario = new {
                usuario.IdUsuario,
                usuario.user,
                usuario.rol,
                usuario.Correo,
                usuario.Nombre_apellido,
                usuario.DNI,
                usuario.Direccion
                },
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

    }
}



