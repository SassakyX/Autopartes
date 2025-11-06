using Back_Repuestos.Controllers;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
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
            
            var context = TestHelper.NewInMemoryDb();
            var SERVICIO = new ProductoServicio(context);

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
            var creado = await SERVICIO.CrearAsync(dto);

            // Assert
            Assert.IsNotNull(creado);  // se creo
            Assert.AreEqual("Aceite Castrol 10W-40", creado.Nombre);
            Assert.AreEqual(10, creado.Stock);

            // También puedes validar que se guardó en la DB
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

            var SERVICIO = new ProductoServicio(context);
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
            var resultado = await SERVICIO.EditarAsync(producto.idProducto, dto);

   
            // Assert
            Assert.IsNotNull(resultado);  // verificamos que no sea nulo
            Assert.AreEqual("Filtro de aire premium", resultado.Nombre);
            Assert.AreEqual(8, resultado.Stock);
            Assert.IsNotNull(2, resultado.Categoria);
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

            var SERVICIO = new ProductoServicio(context);

           
            await SERVICIO.EliminarAsync(producto.idProducto);

            // Assert
            var productosRestantes = context.Productos.ToList();
            Assert.AreEqual(0, productosRestantes.Count);
        }
    }
}
