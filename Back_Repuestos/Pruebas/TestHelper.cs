using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    internal class TestHelper
    {
        public class FakeCorreoService : IEmailService
        {   
            public bool llamado { get; private set; }
            public string? ultimoDestino { get; private set; }
            public string? UltimoCodigo { get; private set; }
            public Task EnviarCodigoAsync(string destino, string codigo)
            {
                llamado = true;
                ultimoDestino = destino;
                UltimoCodigo = codigo;
                return Task.CompletedTask;
            }
        }
        public static AppDbContext NewInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // db única por test
                .Options;

            return new AppDbContext(options);
        }
        public static async Task SeedVentasData(AppDbContext ctx)
        {
            try
            {
                var usuario = new Usuario
                {
                    IdUsuario = 1,
                    Nombre_apellido = "Carlos Ramos",
                    DNI = "76618821",
                    Direccion = "Av. Siempre Viva 123",
                    Correo = "carlos@correo.com",
                    user = "carlosr",
                    Contrasenia = "12345",
                    rol = "Cliente"
                };

            var producto = new Productos
            {
                idProducto = 1,
                Nombre = "Filtro 1",
                PrecioVena = 15
            };
            ctx.Usuarios.Add(usuario);
            ctx.Productos.Add(producto);
            await ctx.SaveChangesAsync();
            var venta = new Venta
            {
                IdVenta = 1,
                IdUsuario = 1,
                Fecha = DateTime.Now,
                Total = 30,
                Estado = "Completado",
                Usuario = usuario,
                DetalleVentas = new List<DetalleVenta>
            {
                new DetalleVenta
                {
                    IdProducto = 1,
                    Cantidad = 2,
                    Precio_unidad = 15,
                    Subtotal = 30,
                    Producto = producto
                }
            }
            };
            ctx.Ventas.Add(venta);
            await ctx.SaveChangesAsync();
        }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SeedVentasData: {ex.GetType().Name} → {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
                throw;
            }

        }
    }
}

