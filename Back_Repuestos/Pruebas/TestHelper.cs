using Back_Repuestos.Data;
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
    }
}
