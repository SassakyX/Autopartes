using Back_Repuestos.Controllers;
using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{


        [TestClass]
        public class CategoriaControllerTests
        {
            private AppDbContext _context;
            private CategoriaServicio _service;
            private CategoriaController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Para este test usamos una base de datos limbia
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new CategoriaServicio(_context);
            _controller = new CategoriaController(_service);
        }

        [TestMethod]
        public async Task Crear_DeberiaAgregarCategoria()
        {
            // Arrange
            var categoria = new Categoria { Nombre = "Repuestos Eléctricos" };

            // Act
            var result = await _controller.Crear(categoria);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode ?? 200);
            Assert.AreEqual(1, _context.Categorias.Count());
        }

        [TestMethod]
        public async Task GetAll_DeberiaRetornarCategorias()
        {
            // Arrange
            _context.Categorias.Add(new Categoria { Nombre = "Motor" });
            _context.Categorias.Add(new Categoria { Nombre = "Frenos" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCategorias();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var categorias = okResult.Value as IEnumerable<object>;
            Assert.AreEqual(2, categorias.Count());
        }

        [TestMethod]
        public async Task Editar_DeberiaActualizarNombre()
        {
            // Arrange
            var categoria = new Categoria { Nombre = "Viejo" };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var nuevaCategoria = new Categoria { Nombre = "Nuevo" };

            // Act
            var result = await _controller.Editar(categoria.IdCategoria, nuevaCategoria);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualizado = okResult.Value as Categoria;
            Assert.AreEqual("Nuevo", actualizado.Nombre);
        }

    }
}

