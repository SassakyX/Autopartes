using Back_Repuestos.Controllers;
using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
   
    
        [TestClass]
        public class VentasControllerTests
        {


        private ILogger<VentasService> CrearLogger()
        {
            return new Logger<VentasService>(new LoggerFactory());
        }

        [TestMethod]
        public async Task CrearVenta_DeberiaGuardarCorrectamente()
        {
            var ctx = TestHelper.NewInMemoryDb();
            await TestHelper.SeedVentasData(ctx); // ya mete usuarios y productos
            await ctx.SaveChangesAsync();

            // 1️⃣ Obtener usuario y producto del seed
            var usuario = await ctx.Usuarios.FirstAsync();
            var producto = await ctx.Productos.FirstAsync(p => p.idProducto == 1);

            // 2️⃣ Crear servicio con NullLogger para evitar error de logger null
            var service = new VentasService(ctx, NullLogger<VentasService>.Instance);

            // 3️⃣ DTO usando IDs reales del seed
            var dto = new PedidoCrearDTO
            {
                IdUsuario = usuario.IdUsuario,
                Detalles = new List<PedidoDetalleCrearDTO>
        {
            new PedidoDetalleCrearDTO
            {
                IdProducto = producto.idProducto,
                Cantidad = 2,
                PrecioUnidad = 20
            }
            }
            };

            // 4️⃣ Crear venta
            var result = await service.CrearVentaAsync(dto);

            // 5️⃣ Validaciones
            var venta = await ctx.Ventas.Include(v => v.DetalleVentas).FirstOrDefaultAsync();
            Assert.IsNotNull(venta);
            Assert.AreEqual(usuario.IdUsuario, venta.IdUsuario);
            Assert.AreEqual(2 * producto.PrecioVena, venta.Total);
            Assert.AreEqual(1, venta.DetalleVentas.Count);
        }

        [TestMethod]
        public async Task CambiarEstado_Finalizado_DeberiaActualizarStock()
        {
            // Arrange
            var ctx = TestHelper.NewInMemoryDb();

            var producto = new Productos
            {
                idProducto = 1,
                Nombre = "Repuesto B",
                stock = 5,
                PrecioVena = 50
            };
            ctx.Productos.Add(producto);

            var venta = new Venta
            {
                IdUsuario = 1,
                Estado = "Pendiente",
                Fecha = DateTime.UtcNow,
                Total = 100,
                DetalleVentas = new List<DetalleVenta>
            {
                new DetalleVenta { IdProducto = 1, Cantidad = 2, Precio_unidad = 50, Subtotal = 100 }
            }
            };
            ctx.Ventas.Add(venta);
            await ctx.SaveChangesAsync();

            var service = new VentasService(ctx, CrearLogger());

            // Act
            var result = await service.CambiarEstadoAsync(venta.IdVenta, "Finalizado");

            // Assert
            Assert.IsTrue(result.Exito);
            var productoActualizado = await ctx.Productos.FindAsync(1);
            Assert.AreEqual(3, productoActualizado.stock);

            var ventaActualizada = await ctx.Ventas.FindAsync(venta.IdVenta);
            Assert.AreEqual("Finalizado", ventaActualizada.Estado);
        }
    }
     }
