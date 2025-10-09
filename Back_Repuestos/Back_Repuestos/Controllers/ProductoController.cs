using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTodos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Select(p => new {
                    p.idProducto,
                    p.Nombre,
                    p.Descrpicion,
                    p.PrecioCompra,
                    p.PrecioVena,
                    p.stock,
                    p.IdCategoria,
                    Categoria = p.Categoria != null ? p.Categoria.Nombre : null,
                    ImagenBase64 = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null
                })
                .ToListAsync();

            return Ok(productos);
        }


        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<object>>> GetFiltrados(
            string? nombre,
            int? idCategoria,
            decimal? precioMin,
            decimal? precioMax,
            bool? soloStock)
        {
            var query = _context.Productos
                .Include(p => p.Categoria) // 🔹 jala la relación
                .AsQueryable();

            // 🔹 Filtros dinámicos
            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (idCategoria.HasValue)
                query = query.Where(p => p.IdCategoria == idCategoria);

            if (precioMin.HasValue)
                query = query.Where(p => p.PrecioVena >= precioMin);

            if (precioMax.HasValue)
                query = query.Where(p => p.PrecioVena <= precioMax);

            if (soloStock.HasValue && soloStock.Value)
                query = query.Where(p => p.stock > 0);

            var productos = await query.Select(p => new {
                p.idProducto,
                p.Nombre,
                p.Descrpicion,
                p.PrecioCompra,
                p.PrecioVena,
                p.stock,
                Categoria = p.Categoria != null ? p.Categoria.Nombre : null,
                ImagenBase64 = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null
            }).ToListAsync();

            return Ok(productos);
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] CrearProductoDTO dto)
        {
            var producto = new Productos
            {
                Nombre = dto.Nombre,
                Descrpicion = dto.Descrpicion,
                PrecioCompra = dto.PrecioCompra,
                PrecioVena = dto.PrecioVena,
                stock = dto.Stock,
                IdCategoria = dto.IdCategoria
            };

            if (dto.Imagen != null)
            {
                using var ms = new MemoryStream();
                await dto.Imagen.CopyToAsync(ms);
                producto.Imagen = ms.ToArray();
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto creado" });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromForm] CrearProductoDTO dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.Nombre = dto.Nombre;
            producto.Descrpicion = dto.Descrpicion;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.PrecioVena = dto.PrecioVena;
            producto.stock = dto.Stock;
            producto.IdCategoria = dto.IdCategoria;

            if (dto.Imagen != null)
            {
                using var ms = new MemoryStream();
                await dto.Imagen.CopyToAsync(ms);
                producto.Imagen = ms.ToArray();
            }

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto actualizado" });
        }



        // DELETE: api/Producto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto eliminado" });
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPorId(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.idProducto == id);

            if (producto == null) return NotFound();

            return new
            {
                producto.idProducto,
                producto.Nombre,
                producto.Descrpicion,
                producto.PrecioCompra,
                producto.PrecioVena,
                producto.stock,
                Categoria = producto.Categoria != null ? producto.Categoria.Nombre : null,
                ImagenBase64 = producto.Imagen != null ? Convert.ToBase64String(producto.Imagen) : null
            };
        }

    }
}

