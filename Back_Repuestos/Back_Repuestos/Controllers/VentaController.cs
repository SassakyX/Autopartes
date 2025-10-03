using Back_Repuestos.Data;
using Microsoft.AspNetCore.Mvc;
using Back_Repuestos.DTO;
using Microsoft.EntityFrameworkCore;
using Back_Repuestos.Modelos;


namespace Back_Repuestos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearVenta([FromBody] PedidoCrearDTO request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.IdUsuario);
            if (usuario == null)
                return BadRequest(new { mensaje = "Usuario no encontrado" });

            var venta = new Venta
            {
                Fecha = DateTime.UtcNow,
                IdUsuario = usuario.IdUsuario,
                Total = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnidad),
                Estado = "Pendiente",
                DetalleVentas = request.Detalles.Select(d => new DetalleVenta
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Precio_unidad = d.PrecioUnidad,
                    Subtotal = d.Cantidad * d.PrecioUnidad
                }).ToList()
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada correctamente", venta.IdVenta, venta.Estado });
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            // Si ya está finalizado o cancelado, ya no podria mopdificar
            if (venta.Estado == "Finalizado" || venta.Estado == "Cancelado")
                return BadRequest(new { mensaje = "Esta venta ya no se puede modificar." });

            // Si se intenta pasar a Finalizado, verificar stock primero
            if (nuevoEstado == "Finalizado")
            {
                foreach (var det in venta.DetalleVentas!)
                {
                    if (det.Producto == null) continue;

                    if (det.Producto.stock < det.Cantidad)
                    {
                        return BadRequest(new
                        {
                            mensaje = $"Stock insuficiente para el producto '{det.Producto.Nombre}'. " +
                                      $"Stock actual: {det.Producto.stock}, requerido: {det.Cantidad}"
                        });
                    }
                }

                // Si hay stock suficiente, descontamos
                foreach (var det in venta.DetalleVentas!)
                {
                    if (det.Producto != null)
                    {
                        det.Producto.stock -= det.Cantidad;
                    }
                }
            }

            // Actualizamos estado
            venta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Estado cambiado a {nuevoEstado}" });
        }



        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .Select(v => new PedidoDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Estado = v.Estado,
                    UsuarioNombre = v.Usuario != null ? v.Usuario.Nombre_apellido : "Desconocido",
                    UsuarioCorreo = v.Usuario != null ? v.Usuario.Correo : string.Empty,
                    Detalles = v.DetalleVentas!.Select(d => new DetalleVentaDTO
                    {
                        Cantidad = d.Cantidad,
                        PrecioUnidad = d.Precio_unidad,
                        Subtotal = d.Subtotal,
                        ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Sin nombre"
                    }).ToList()
                })
                .ToListAsync();

            return Ok(ventas);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetVentasPorUsuario(int idUsuario)
        {
            var ventas = await _context.Ventas
                .Where(v => v.IdUsuario == idUsuario)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .Select(v => new PedidoDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Estado = v.Estado,
                    UsuarioNombre = v.Usuario != null ? v.Usuario.Nombre_apellido : "Desconocido",
                    UsuarioCorreo = v.Usuario != null ? v.Usuario.Correo : string.Empty,
                    Detalles = v.DetalleVentas!.Select(d => new DetalleVentaDTO
                    {
                        Cantidad = d.Cantidad,
                        PrecioUnidad = d.Precio_unidad,
                        Subtotal = d.Subtotal,
                        ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Sin nombre"
                    }).ToList()
                })
                .ToListAsync();

            if (!ventas.Any())
                return NotFound(new { mensaje = "No se encontraron ventas para este usuario" });

            return Ok(ventas);
        }
    }
}