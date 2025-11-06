using Back_Repuestos;
using Back_Repuestos.Controllers;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Back_Repuestos.Services;
using Microsoft.AspNetCore.Mvc;
using static Pruebas.TestHelper;

namespace Pruebas

{
    [TestClass]
    public sealed class Test1
      
    {
        public TestContext TestContext { get; set; } = null!;
        [TestMethod]
        public async Task PruebaLoginTRUE()
        {
            
            // Memoria almacenada en ram
            var ctx = TestHelper.NewInMemoryDb();

            // Servico Falso 
            var correo = new TestHelper.FakeCorreoService();

            // Controlador real con dependencias falsas
            var Servicio = new AuthServicio(ctx, correo);

            // Sembrar usuario en la ram
            var user = new Usuario
            {
                Nombre_apellido = "Carlos",
                DNI = "12345678",
                Correo = "admin@test.com",
                user = "admin",
                Contrasenia = BCrypt.Net.BCrypt.HashPassword("Admin123$"),
                rol = "Admin"
            };
            ctx.Usuarios.Add(user);
            await ctx.SaveChangesAsync();

            //  Crear DTO con credenciales
            var dto = new LoginDTO { User = "admin", Contrasenia = "Admin123$" };

            //  Ejecutar acción
            var result = await Servicio.LoginAsync(dto);

            //  Verificar resultado
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var again = await ctx.Usuarios.FindAsync(user.IdUsuario);
            Assert.IsNotNull(again!.CodigoVerificacion, "Debe generar OTP");
            Assert.IsNotNull(again.CodigoExpira, "Debe setear fecha de expiración");
            Assert.IsTrue(again.CodigoExpira > System.DateTime.UtcNow, "Expiración debe ser futura (≈ +5min)");

            // y que el correo “se envió”
            Assert.IsTrue(correo.llamado, "Debe intentar enviar correo");
            Assert.AreEqual("admin@test.com", correo.ultimoDestino);
            Assert.IsFalse(string.IsNullOrWhiteSpace(correo.UltimoCodigo));


            TestContext.WriteLine($"OTP generado: {again.CodigoVerificacion}");
            TestContext.WriteLine($"Expira: {again.CodigoExpira}");
            TestContext.WriteLine($"Correo enviado a: {correo.ultimoDestino} con código {correo.UltimoCodigo}");
        }
        //Falso
        [TestMethod]
        public async Task Login_Unauthorized_CuandoUsuarioNoExiste()
        {
            var ctx = TestHelper.NewInMemoryDb();
            var Correo = new FakeCorreoService();
            var Servicio = new AuthServicio(ctx, Correo);

            var dto = new LoginDTO { User = "nadie", Contrasenia = "123" };

            var result = await Servicio.LoginAsync(dto);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
            Assert.IsFalse(Correo.llamado);
        }
    }
}
