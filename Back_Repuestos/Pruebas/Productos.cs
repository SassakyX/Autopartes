using Back_Repuestos.Controllers;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    [TestClass]
    public class ProductosTests
    {
        [TestMethod]
        public async Task Crear_DeberiaGuardarProductoCorrectamente()
        {
            // Arrange
            var context = TestHelper.NewInMemoryDb();
            var controller = new ProductoController(context);

            var dto = new CrearProductoDTO
            {
                Nombre = "Aceite Castrol 10W-40",
                Descrpicion = "Lubricante sintético de alta calidad",
                PrecioCompra = 25,
                PrecioVena = 35,
                Stock = 10,
                IdCategoria = 1
            };

            // Act
            var resultado = await controller.Crear(dto);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
            var ok = resultado as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual("Producto creado", ok.Value.GetType().GetProperty("mensaje")?.GetValue(ok.Value));

            var guardado = context.Productos.FirstOrDefault(p => p.Nombre == "Aceite Castrol 10W-40");
            Assert.IsNotNull(guardado);
            Assert.AreEqual(10, guardado.stock);
        }

        [TestMethod]
        public async Task Editar_ActualizarProducto()
        {
            // Arrange
            var context = TestHelper.NewInMemoryDb();
            var producto = new Productos
            {
                Nombre = "Filtro de aire",
                Descrpicion = "Filtro básico",
                PrecioCompra = 15,
                PrecioVena = 25,
                stock = 5,
                IdCategoria = 1
            };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            var controller = new ProductoController(context);
            var dto = new CrearProductoDTO
            {
                Nombre = "Filtro de aire premium",
                Descrpicion = "Filtro mejorado con más capas",
                PrecioCompra = 20,
                PrecioVena = 30,
                Stock = 8,
                IdCategoria = 2
            };

            // Act
            var resultado = await controller.Editar(producto.idProducto, dto);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
            var actualizado = context.Productos.First();
            Assert.AreEqual("Filtro de aire premium", actualizado.Nombre);
            Assert.AreEqual(8, actualizado.stock);
            Assert.AreEqual(2, actualizado.IdCategoria);
        }

        [TestMethod]
        public async Task Eliminar_DeberiaEliminarProductoCorrectamente()
        {
            // Arranque
            var context = TestHelper.NewInMemoryDb();
            var producto = new Productos
            {
                Nombre = "Pastillas de freno",
                Descrpicion = "Juego de pastillas delanteras",
                PrecioCompra = 50,
                PrecioVena = 70,
                stock = 3,
                IdCategoria = 1
            };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            var controller = new ProductoController(context);

            // Act
            var resultado = await controller.Eliminar(producto.idProducto);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
            var productosRestantes = context.Productos.ToList();
            Assert.AreEqual(0, productosRestantes.Count);
        }
    }
}
