using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class ResenasController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ResenasController(AppDbContext context)
            {
                _context = context;
            }

            [HttpPost]
            public async Task<IActionResult> CrearReseña([FromBody] ReseñaDto dto)
            {
            // Verificamos si el usuario ha comprado el producto
            var detalles = await _context.DetalleVentas
            .Include(dv => dv.Venta)
            .ToListAsync();
            var haComprado = await _context.Ventas
                .AnyAsync(v =>
                    v.IdUsuario == dto.UsuarioId &&
                    v.Estado == "Finalizado" &&
                    v.DetalleVentas.Any(dv => dv.IdProducto == dto.ProductoId)
                );
            if (!haComprado)
                    return BadRequest("Solo los usuarios que han comprado este producto pueden calificarlo.");
                

            var reseña = new Resena
                {
                    ProductoId = dto.ProductoId,
                    UsuarioId = dto.UsuarioId,
                    Estrellas = dto.Estrellas,
                    Comentario = dto.Comentario,
                    Fecha = DateTime.Now
                };

                _context.Resenas.Add(reseña);
                await _context.SaveChangesAsync();

                return Ok(reseña);
            }

            [HttpGet("{productoId}")]
            public async Task<IActionResult> ObtenerReseñas(int productoId)
            {
                var reseñas = await _context.Resenas
                    .Where(r => r.ProductoId == productoId)
                    .Include(r => r.Usuario)
                    .Select(r => new
                    {
                        r.Id,
                        r.ProductoId,
                        Usuario = r.Usuario != null ? r.Usuario.Nombre_apellido : "Desconocido",
                        r.Estrellas,
                        r.Comentario,
                        r.Fecha
                    })
                    .ToListAsync();

                return Ok(reseñas);
            }
            [HttpGet("haComprado")]
            public async Task<IActionResult> HaComprado(int usuarioId, int productoId)
            {
                var haComprado = await _context.Ventas
                    .AnyAsync(v => v.IdUsuario == usuarioId &&
                                   v.DetalleVentas.Any(dv => dv.IdProducto == productoId));

                return Ok(haComprado);
            }
        }
    }

