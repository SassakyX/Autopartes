using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class ResenasController : ControllerBase
        {
        private readonly ResenaService _resenaServicio;

        public ResenasController(ResenaService resenaServicio)
        {
            _resenaServicio = resenaServicio;
        }

        [HttpPost]
        public async Task<IActionResult> CrearResena([FromBody] ResenaDto dto)
        {
            try
            {
                var resena = await _resenaServicio.CrearResenaAsync(dto);
                return Ok(resena);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{productoId}")]
        public async Task<IActionResult> ObtenerResenas(int productoId)
        {
            var resenas = await _resenaServicio.ObtenerResenasAsync(productoId);
            var respuesta = resenas.Select(r => new
            {
                r.Id,
                r.ProductoId,
                Usuario = r.Usuario != null ? r.Usuario.Nombre_apellido : "Desconocido",
                r.Estrellas,
                r.Comentario,
                r.Fecha
            });
            return Ok(respuesta);
        }

        [HttpGet("haComprado")]
        public async Task<IActionResult> HaComprado(int usuarioId, int productoId)
        {
            var haComprado = await _resenaServicio.HaCompradoAsync(usuarioId, productoId);
            return Ok(haComprado);
        }
    }
}

