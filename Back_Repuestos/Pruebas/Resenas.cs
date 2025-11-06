using Back_Repuestos.Controllers;
using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Pruebas
{

    [TestClass]
    public class ResenasTests
    {
        private AppDbContext GetInMemoryDb()
        {
            return TestHelper.NewInMemoryDb();
        }

        private ResenaService CrearServicio(AppDbContext context)
        {
            var loggerMock = new Mock<ILogger<ResenaService>>();
            return new ResenaService(context, loggerMock.Object);
        }

        [TestMethod]
        public async Task CrearResena_DeberiaFallar_SiNoHaComprado()
        {
            // Arrange
            var context = GetInMemoryDb();

            // Sembramos usuario, productos y ventas
            await TestHelper.SeedVentasData(context);

            var service = CrearServicio(context);

            var dto = new ResenaDto
            {
                UsuarioId = 1,       // Usuario que ya existe
                ProductoId = 2,    // Producto que no compró
                Estrellas = 4,
                Comentario = "Buen producto"
                
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await service.CrearResenaAsync(dto);
            });
        }

        [TestMethod]
        public async Task CrearResena_DeberiaCrearResena_SiUsuarioHaComprado()
        {
            // Arrange
            var context = GetInMemoryDb();

            // Sembramos usuario, productos y ventas
            await TestHelper.SeedVentasData(context);

            var service = CrearServicio(context);

            var dto = new ResenaDto
            {
                UsuarioId = 1,      // Usuario que existe y compró producto 100
                ProductoId = 1,
                Estrellas = 5,
                Comentario = "Excelente compra"
            };

            // Act
            var resultado = await service.CrearResenaAsync(dto);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(5, resultado.Estrellas);
            Assert.AreEqual("Excelente compra", resultado.Comentario);
            Assert.AreEqual(1, resultado.UsuarioId);

            var cantidad = await context.Resenas.CountAsync();
            Assert.AreEqual(1, cantidad);
        }
    }
}