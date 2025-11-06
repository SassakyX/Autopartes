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
    public class VentasController : ControllerBase
    {
        private readonly VentasService _ventaService;
        private readonly ILogger<VentasController> _logger;

        public VentasController(VentasService ventaService, ILogger<VentasController> logger)
        {
            _ventaService = ventaService;
            _logger = logger;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearVenta([FromBody] PedidoCrearDTO request)
        {
            try
            {
                var venta = await _ventaService.CrearVentaAsync(request);
                return Ok(new { mensaje = "Venta registrada correctamente", venta.IdVenta, venta.Estado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en creación de venta");
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                var resultado = await _ventaService.CambiarEstadoAsync(id, nuevoEstado);
                if (!resultado.Exito) return BadRequest(new { mensaje = resultado.Mensaje });
                return Ok(new { mensaje = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de venta");
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            try
            {
                var ventas = await _ventaService.ObtenerVentasAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas");
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetVentasPorUsuario(int idUsuario)
        {
            try
            {
                var ventas = await _ventaService.ObtenerVentasPorUsuarioAsync(idUsuario);
                if (!ventas.Any()) return NotFound(new { mensaje = "No se encontraron ventas para este usuario" });
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas por usuario");
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }
}