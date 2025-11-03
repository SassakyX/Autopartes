using Back_Repuestos.Controllers;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Pruebas
{

    [TestClass]
    public class ResenasTests
    {
        [TestMethod]
        public async Task CrearResena_BadRequest_SiNoHaComprado()
        {
            // Arranca
            var context = TestHelper.NewInMemoryDb();
            var controller = new ResenasController(context);

            var dto = new ReseñaDto
            {
                UsuarioId = 1,
                ProductoId = 100,
                Estrellas = 4,
                Comentario = "Buen producto"
            };

            // Act
            var resultado = await controller.CrearReseña(dto);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));

            var badRequest = resultado as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Solo los usuarios que han comprado este producto pueden calificarlo.", badRequest.Value);
        }

        [TestMethod]
        public async Task CrearResena_DeberiaCrearResena_SiUsuarioHaComprado()
        {
            // Arranca
            var context = TestHelper.NewInMemoryDb();

            // Simulamos venta existente en la ram 
            var venta = new Venta
            {
                IdUsuario = 1,
                Estado = "Finalizado",
                DetalleVentas = new List<DetalleVenta>
                {
                    new DetalleVenta { IdProducto = 100, Cantidad = 1, Precio_unidad = 50 }
                }
            };
            context.Ventas.Add(venta);
            await context.SaveChangesAsync();

            var controller = new ResenasController(context);

            var dto = new ReseñaDto
            {
                UsuarioId = 1,
                ProductoId = 100,
                Estrellas = 5,
                Comentario = "Excelente compra"
            };

         
            var resultado = await controller.CrearReseña(dto);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));

            var ok = resultado as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.IsInstanceOfType(ok.Value, typeof(Resena));

            var resena = ok.Value as Resena;
            Assert.AreEqual(5, resena.Estrellas);
            Assert.AreEqual("Excelente compra", resena.Comentario);
            Assert.AreEqual(1, resena.UsuarioId);

            // Confirmar 
            var cantidad = context.Resenas.Count();
            Assert.AreEqual(1, cantidad);
        }
    }
}


