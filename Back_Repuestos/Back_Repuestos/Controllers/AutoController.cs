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
        private readonly AuthServicio _auth;

        public AutoController(AuthServicio auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOregistro dto)
            => await _auth.RegisterAsync(dto);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
            => await _auth.LoginAsync(dto);

        [HttpPost("verificar-codigo")]
        public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoDTO dto, [FromServices] IConfiguration cfg)
            => await _auth.VerificarCodigoAsync(dto, cfg);
    }
}






