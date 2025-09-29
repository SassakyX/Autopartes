using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Dato_semilla
{
    public class DatoSemilla
    {
        public static async Task SeedAdminAsync(AppDbContext db, IConfiguration config)
        {
            var email = config["AdminSeed:Correo"] ?? "Sassaky_@outlook.com";

            // verificacion de existencia
            if (await db.Usuarios.AnyAsync(u => u.Correo == email))
                return;

            var admin = new Usuario
            {
                Nombre_apellido = "Carlos Ramos",
                DNI = "76618821",
                Direccion = "Oficina central",
                Correo = email,
                user = "admin",
                Contrasenia = BCrypt.Net.BCrypt.HashPassword("Admin123$"),
                rol = "Admin"
            };

            db.Usuarios.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
