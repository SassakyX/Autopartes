using Back_Repuestos.Data;
using Back_Repuestos.DTO;
using Back_Repuestos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Services
{
    public class ProductoServicio
    {

        private readonly AppDbContext _context;

        public ProductoServicio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DTOproducto>> GetTodosAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Select(p => new DTOproducto
                {
                    IdProducto = p.idProducto,
                    Nombre = p.Nombre,
                    Descrpicion = p.Descrpicion,
                    PrecioCompra = p.PrecioCompra,
                    PrecioVena = p.PrecioVena,
                    Stock = p.stock,
                    Categoria = p.Categoria != null ? p.Categoria.Nombre : null,
                    ImagenBase64 = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null
                })
                .ToListAsync();
        }

        public async Task<List<DTOproducto>> GetFiltradosAsync(string? nombre, int? idCategoria, decimal? precioMin, decimal? precioMax, bool? soloStock)
        {
            var query = _context.Productos.Include(p => p.Categoria).AsQueryable();

            if (!string.IsNullOrEmpty(nombre)) query = query.Where(p => p.Nombre.Contains(nombre));
            if (idCategoria.HasValue) query = query.Where(p => p.IdCategoria == idCategoria);
            if (precioMin.HasValue) query = query.Where(p => p.PrecioVena >= precioMin);
            if (precioMax.HasValue) query = query.Where(p => p.PrecioVena <= precioMax);
            if (soloStock.HasValue && soloStock.Value) query = query.Where(p => p.stock > 0);

            return await query.Select(p => new DTOproducto
            {
                IdProducto = p.idProducto,
                Nombre = p.Nombre,
                Descrpicion = p.Descrpicion,
                PrecioCompra = p.PrecioCompra,
                PrecioVena = p.PrecioVena,
                Stock = p.stock,
                Categoria = p.Categoria != null ? p.Categoria.Nombre : null,
                ImagenBase64 = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null
            }).ToListAsync();
        }

        public async Task<DTOproducto> CrearAsync(CrearProductoDTO dto)
        {
            if (await _context.Productos.AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower()))
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");

            var producto = new Productos
            {
                Nombre = dto.Nombre,
                Descrpicion = dto.Descrpicion,
                PrecioCompra = dto.PrecioCompra,
                PrecioVena = dto.PrecioVena,
                stock = dto.Stock,
                IdCategoria = dto.IdCategoria
            };

            if (dto.Imagen != null)
            {
                using var ms = new MemoryStream();
                await dto.Imagen.CopyToAsync(ms);
                producto.Imagen = ms.ToArray();
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return new DTOproducto
            {
                IdProducto = producto.idProducto,
                Nombre = producto.Nombre,
                Descrpicion = producto.Descrpicion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVena = producto.PrecioVena,
                Stock = producto.stock,
                Categoria = (await _context.Categorias.FindAsync(producto.IdCategoria))?.Nombre,
                ImagenBase64 = producto.Imagen != null ? Convert.ToBase64String(producto.Imagen) : null
            };
        }

        public async Task<DTOproducto> EditarAsync(int id, CrearProductoDTO dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) throw new KeyNotFoundException("Producto no encontrado");

            if (await _context.Productos.AnyAsync(p => p.idProducto != id && p.Nombre.ToLower() == dto.Nombre.ToLower()))
                throw new InvalidOperationException("Ya existe otro producto con ese nombre.");

            producto.Nombre = dto.Nombre;
            producto.Descrpicion = dto.Descrpicion;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.PrecioVena = dto.PrecioVena;
            producto.stock = dto.Stock;
            producto.IdCategoria = dto.IdCategoria;

            if (dto.Imagen != null)
            {
                using var ms = new MemoryStream();
                await dto.Imagen.CopyToAsync(ms);
                producto.Imagen = ms.ToArray();
            }

            await _context.SaveChangesAsync();

            return new DTOproducto
            {
                IdProducto = producto.idProducto,
                Nombre = producto.Nombre,
                Descrpicion = producto.Descrpicion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVena = producto.PrecioVena,
                Stock = producto.stock,
                Categoria = (await _context.Categorias.FindAsync(producto.IdCategoria))?.Nombre,
                ImagenBase64 = producto.Imagen != null ? Convert.ToBase64String(producto.Imagen) : null
            };
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) throw new KeyNotFoundException("Producto no encontrado");

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<DTOproducto> GetPorIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.idProducto == id);

            if (producto == null) throw new KeyNotFoundException("Producto no encontrado");

            return new DTOproducto
            {
                IdProducto = producto.idProducto,
                Nombre = producto.Nombre,
                Descrpicion = producto.Descrpicion,
                PrecioCompra = producto.PrecioCompra,
                PrecioVena = producto.PrecioVena,
                Stock = producto.stock,
                Categoria = producto.Categoria?.Nombre,
                ImagenBase64 = producto.Imagen != null ? Convert.ToBase64String(producto.Imagen) : null
            };
        }
    }
}
