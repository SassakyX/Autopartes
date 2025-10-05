using System;
using System.Collections.Generic;
using System.Linq;
using Back_Repuestos;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Back_Repuestos.Modelos;
using Back_Repuestos.Controllers;
using Back_Repuestos.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Pruebas
{
    [TestClass]
    public class ventasIntegracionPrueba
    {
        [TestMethod]
        public async Task Crearventa_DeberiaguardarConDetalles()
        {
            var ctx = TestHelper.NewInMemoryDb();
            ctx.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                Nombre_apellido = "Carlos Ramos",
                Correo = "Test@correo.com",
                DNI = "76618821",
                user = "Carlos",
                Contrasenia = "1234",
                rol = "Cliente",


            });
            ctx.Productos.Add(new Productos
            {
                idProducto = 1,
                Nombre = "Repuesto A",
                stock = 10,
                PrecioVena = 100

            });
            await ctx.SaveChangesAsync();

            var controller = new VentasController(ctx);

            var dto = new PedidoCrearDTO
            {
                IdUsuario = 1,
                Detalles = new List<PedidoDetalleCrearDTO>
                {
                    new PedidoDetalleCrearDTO
                    {
                        IdProducto = 1,
                        Cantidad = 2,
                        PrecioUnidad = 100,

                    }
                }

            };


            var result = await controller.CrearVenta(dto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);


            var venta = await ctx.Ventas.Include(v => v.DetalleVentas).FirstOrDefaultAsync();
            Assert.IsNotNull(venta);
            Assert.AreEqual(1, venta.IdUsuario);
            Assert.AreEqual(200, venta.Total);
            Assert.AreEqual("Pendiente", venta.Estado);
            Assert.AreEqual(1, venta.DetalleVentas.Count);
        }

        [TestMethod]
        public async Task Cambioestado_finalizado ()
        {
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
                    new DetalleVenta {IdProducto = 1, Cantidad = 2, Precio_unidad = 50, Subtotal = 100}
                }

            };
            ctx.Ventas.Add(venta);
            await ctx.SaveChangesAsync();

            var controller = new VentasController(ctx);

            var result = await controller.CambiarEstado(venta.IdVenta, "Finalizado") as OkObjectResult;


            Assert.IsNotNull(result);
            var actualizado = await ctx.Productos.FindAsync(1);
            Assert.AreEqual(3, actualizado.stock);
            var ventaActualizada = await ctx.Ventas.FindAsync(venta.IdVenta);
            Assert.AreEqual("Finalizado", ventaActualizada.Estado);
        }


    }
}
