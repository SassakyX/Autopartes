using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Services
{
   public class ResenaService
        
    {
         
        private readonly AppDbContext _context;
        private readonly ILogger<ResenaService> _logger;

        public ResenaService(AppDbContext context, ILogger<ResenaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Resena?> CrearResenaAsync(ResenaDto dto)
        {
            try
            {
                // Validar existencia usuario y producto
                if (!await _context.Usuarios.AnyAsync(u => u.IdUsuario == dto.UsuarioId))
                    throw new KeyNotFoundException("Usuario no existe");

                if (!await _context.Productos.AnyAsync(p => p.idProducto == dto.ProductoId))
                    throw new KeyNotFoundException("Producto no existe");

                // Verificar que haya comprado
                bool haComprado = await _context.Ventas
                    .AnyAsync(v => v.IdUsuario == dto.UsuarioId &&
                                   v.Estado == "Finalizado" &&
                                   v.DetalleVentas.Any(dv => dv.IdProducto == dto.ProductoId));

                if (!haComprado)
                    throw new InvalidOperationException("Solo los usuarios que han comprado este producto pueden calificarlo.");

                // Evitar reseña duplicada
                bool yaReseno = await _context.Resenas
                    .AnyAsync(r => r.ProductoId == dto.ProductoId && r.UsuarioId == dto.UsuarioId);

                if (yaReseno)
                    throw new InvalidOperationException("Ya calificaste este producto.");

                var resena = new Resena
                {
                    ProductoId = dto.ProductoId,
                    UsuarioId = dto.UsuarioId,
                    Estrellas = dto.Estrellas,
                    Comentario = dto.Comentario,
                    Fecha = DateTime.Now
                };

                _context.Resenas.Add(resena);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reseña creada por usuario {UsuarioId} para producto {ProductoId}", dto.UsuarioId, dto.ProductoId);

                return resena;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reseña para usuario {UsuarioId} y producto {ProductoId}", dto.UsuarioId, dto.ProductoId);
                throw;
            }
        }

        public async Task<List<Resena>> ObtenerResenasAsync(int productoId)
        {
            return await _context.Resenas
                .Where(r => r.ProductoId == productoId)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        public async Task<bool> HaCompradoAsync(int usuarioId, int productoId)
        {
            return await _context.Ventas
                .AnyAsync(v => v.IdUsuario == usuarioId &&
                               v.DetalleVentas.Any(dv => dv.IdProducto == productoId));
        }
    }
}

