using Back_Repuestos.Controllers;
using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Services
{
    public class VentasService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VentasService> _logger; // <-- logger específico

        public VentasService(AppDbContext context, ILogger<VentasService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public VentasService(AppDbContext ctx)
        {
        }

        public async Task<Venta> CrearVentaAsync(PedidoCrearDTO request)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(request.IdUsuario);
                if (usuario == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                // Validar que existan todos los productos
                var productosIds = request.Detalles.Select(d => d.IdProducto).ToList();
                var productos = await _context.Productos
                    .Where(p => productosIds.Contains(p.idProducto))
                    .ToListAsync();

                if (productos.Count != productosIds.Count)
                    throw new InvalidOperationException("Alguno de los productos no existe");



                var venta = new Venta
                {
                    Fecha = DateTime.UtcNow,
                    IdUsuario = usuario.IdUsuario,
                    Estado = "Pendiente",
                    Total = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnidad),
                    DetalleVentas = request.Detalles.Select(d =>
                    {
                        var prod = productos.First(p => p.idProducto == d.IdProducto);
                        return new DetalleVenta
                        {
                            IdProducto = d.IdProducto,
                            Cantidad = d.Cantidad,
                            Precio_unidad = d.PrecioUnidad,
                            Subtotal = d.Cantidad * d.PrecioUnidad
                        };
                    }).ToList()
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();


                _logger.LogInformation("Venta {id} creada por usuario {user}", venta.IdVenta, usuario.IdUsuario);

                return venta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en creación de venta");
                throw;
            }
        }

        public async Task<(bool Exito, string Mensaje)> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id);


            if (venta == null) return (false, "Venta no encontrada");
            if (venta.Estado == "Finalizado" || venta.Estado == "Cancelado")
                return (false, "Esta venta ya no se puede modificar.");

            if (nuevoEstado == "Finalizado")
            {
                foreach (var det in venta.DetalleVentas)
                {
                    if (det.Producto == null) continue;
                    if (det.Producto.stock < det.Cantidad)
                        throw new InvalidOperationException($"Stock insuficiente para '{det.Producto.Nombre}'. Stock: {det.Producto.stock}, requerido: {det.Cantidad}");
                }

                foreach (var det in venta.DetalleVentas)
                {
                    if (det.Producto != null)
                        det.Producto.stock -= det.Cantidad;
                }
            }

            venta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return (true, $"Estado cambiado a {nuevoEstado}");

        }

        public async Task<List<PedidoDTO>> ObtenerVentasAsync()
        {
            return await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .Select(v => new PedidoDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Estado = v.Estado,
                    IdUsuario = v.IdUsuario,
                    UsuarioNombre = v.Usuario != null ? v.Usuario.Nombre_apellido : "Desconocido",
                    UsuarioCorreo = v.Usuario != null ? v.Usuario.Correo : string.Empty,    
                    Detalles = v.DetalleVentas!.Select(static d => new DetalleVentaDTO
                    {
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnidad = d.Precio_unidad,
                        Subtotal = d.Subtotal,
                        ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Sin nombre"
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<PedidoDTO>> ObtenerVentasPorUsuarioAsync(int idUsuario)
        {
            return await _context.Ventas
                .Where(v => v.IdUsuario == idUsuario)
                .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
                .Include(v => v.Usuario)
                .Select(v => new PedidoDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    Estado = v.Estado,
                    IdUsuario = v.IdUsuario,
                    UsuarioNombre = v.Usuario != null ? v.Usuario.Nombre_apellido : "Desconocido",
                    UsuarioCorreo = v.Usuario != null ? v.Usuario.Correo : string.Empty,
                    Detalles = v.DetalleVentas!.Select(d => new DetalleVentaDTO
                    {
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnidad = d.Precio_unidad,
                        Subtotal = d.Subtotal,
                        ProductoNombre = d.Producto != null ? d.Producto.Nombre : "Sin nombre"
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}

