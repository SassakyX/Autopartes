using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_Repuestos.Services
{
        public class AuthServicio
        {
            private readonly AppDbContext _context;
            private readonly IEmailService _correo;

            public AuthServicio(AppDbContext context, IEmailService correo)
            {
                _context = context;
                _correo = correo;
            }

            public async Task<IActionResult> RegisterAsync(DTOregistro request)
            {
                // Validaciones
                if (await _context.Usuarios.AnyAsync(u => u.Correo == request.Correo))
                    return new BadRequestObjectResult(new { mensaje = "El correo ya está registrado." });

                if (await _context.Usuarios.AnyAsync(u => u.user == request.User))
                    return new BadRequestObjectResult(new { mensaje = "El usuario ya está registrado." });

                if (await _context.Usuarios.AnyAsync(u => u.DNI == request.DNI))
                return new BadRequestObjectResult(new { mensaje = "El DNI ya está registrado." });

            // Generar código y hash
            var codigo = new Random().Next(100000, 999999).ToString();
                var hash = BCrypt.Net.BCrypt.HashPassword(request.Contrasenia);

                // Guardar temporal
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
                await _context.SaveChangesAsync();
                await _correo.EnviarCodigoAsync(request.Correo, codigo);

                return new OkObjectResult(new { mensaje = "Se envió un código al correo." });
            }

            public async Task<IActionResult> LoginAsync(LoginDTO request)
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.user == request.User);
                if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, usuario.Contrasenia))
                    return new UnauthorizedObjectResult(new { mensaje = "Usuario o contraseña incorrectos" });

                var otp = Random.Shared.Next(100000, 999999).ToString();
                usuario.CodigoVerificacion = otp;
                usuario.CodigoExpira = DateTime.UtcNow.AddMinutes(5);
                await _context.SaveChangesAsync();

                await _correo.EnviarCodigoAsync(usuario.Correo, otp);
                return new OkObjectResult(new
                {
                    requiereCodigo = true, mensaje = "Código de verificación enviado."      });
            }

            public async Task<IActionResult> VerificarCodigoAsync(VerificarCodigoDTO request, IConfiguration cfg)
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.user == request.User);
                if (usuario != null)
                {
                    if (usuario.CodigoVerificacion != request.Codigo)
                        return new UnauthorizedObjectResult(new { mensaje = "Código inválido." });

                    if (usuario.CodigoExpira < DateTime.UtcNow)
                        return new UnauthorizedObjectResult(new { mensaje = "El código expiró." });

                    usuario.CodigoVerificacion = null;
                    usuario.CodigoExpira = null;
                    await _context.SaveChangesAsync();

                    var token = GenerarJwt(usuario, cfg);
                    return new OkObjectResult(new {
                        usuario = new
                        {
                            IdUsuario = usuario.IdUsuario,
                            user = usuario.user,
                            Nombre_apellido = usuario.Nombre_apellido,
                            rol = usuario.rol
                        },
                        mensaje = "Login exitoso.", token });
                }

                return new UnauthorizedObjectResult(new { mensaje = "Usuario no encontrado." });
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


