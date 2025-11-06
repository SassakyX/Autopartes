using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaServicio _service;

        public CategoriaController(CategoriaServicio service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _service.ObtenerCategoriasAsync();
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Categoria categoria)
        {
            var nueva = await _service.CrearCategoriaAsync(categoria);
            return Ok(nueva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] Categoria categoria)
        {
            var editada = await _service.EditarCategoriaAsync(id, categoria);
            return Ok(editada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var mensaje = await _service.EliminarCategoriaAsync(id);
            return Ok(new { mensaje });
        }
    }
}

