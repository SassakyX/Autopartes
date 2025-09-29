using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new {
                    c.IdCategoria,
                    c.Nombre
                })
                .ToListAsync();

            return Ok(categorias);
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Categoria dto)
        {
            _context.Categorias.Add(dto);
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] Categoria dto)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat == null) return NotFound();
            cat.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat == null) return NotFound();
            _context.Categorias.Remove(cat);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}

