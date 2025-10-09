using Back_Repuestos.Controllers;
using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Http;
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
        public class VentasControllerTests
        {


            [TestMethod]
            public async Task GetVentas_DeberiaRetornarTodasLasVentas()
            {
                var ctx = TestHelper.NewInMemoryDb();
                await TestHelper.SeedVentasData(ctx);
                var controller = new VentasController(ctx);

                // Act
                var result = await controller.GetVentas();

                // Assert
                var ok = result as OkObjectResult;
                var ventas = ok?.Value as List<PedidoDTO>;

                Assert.IsNotNull(ventas);
                Assert.AreEqual(1, ventas.Count);
                Assert.AreEqual("Completado", ventas.First().Estado);
            }

            [TestMethod]
            public async Task GetVentasPorUsuario_DeberiaRetornarSoloLasDelUsuario()
            {
                var ctx = TestHelper.NewInMemoryDb();
                await TestHelper.SeedVentasData(ctx);
                var controller = new VentasController(ctx);

                // Act
                var result = await controller.GetVentasPorUsuario(1);

                // Assert
                var ok = result as OkObjectResult;
                var ventas = ok?.Value as List<PedidoDTO>;

                Assert.IsNotNull(ventas);
                Assert.AreEqual(1, ventas.Count);
                Assert.AreEqual("Carlos Ramos", ventas.First().UsuarioNombre);
            }

            [TestMethod]
            public async Task GetVentasPorUsuario_SiNoHayVentas_DeberiaRetornarNotFound()
            {
                var ctx = TestHelper.NewInMemoryDb(); // no seed porque ahi hay datos, en esta clase  está vacío
                var controller = new VentasController(ctx);

                // Act
                var result = await controller.GetVentasPorUsuario(999);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            }
        }
    }

