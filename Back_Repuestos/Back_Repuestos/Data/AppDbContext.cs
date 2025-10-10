using Back_Repuestos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<UsuarioTemp> UsuariosTemporales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> detalleVentas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();
            // Usuario - Venta (1 a muchos)
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Venta)
                .HasForeignKey(v => v.IdUsuario);

            // Categoria - Producto (1 a muchos)
            modelBuilder.Entity<Productos>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.IdCategoria);

            // Venta - DetalleVenta (1 a muchos)
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(d => d.IdVenta);

            // Producto - DetalleVenta (1 a muchos)
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetalleVentas)
                .HasForeignKey(d => d.IdProducto);
        }


    }
}
