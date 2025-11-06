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
    public class ProductoController : ControllerBase
    {
        private readonly ProductoServicio _service;

        public ProductoController(ProductoServicio service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos() => Ok(await _service.GetTodosAsync());

        [HttpGet("filtrar")]
        public async Task<IActionResult> GetFiltrados(string? nombre, int? idCategoria, decimal? precioMin, decimal? precioMax, bool? soloStock) =>
            Ok(await _service.GetFiltradosAsync(nombre, idCategoria, precioMin, precioMax, soloStock));

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] CrearProductoDTO dto) =>
            Ok(await _service.CrearAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromForm] CrearProductoDTO dto) =>
            Ok(await _service.EditarAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { mensaje = "Producto eliminado" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id) => Ok(await _service.GetPorIdAsync(id));
    }
}

