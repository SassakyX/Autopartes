using Back_Repuestos.Data;
using Back_Repuestos.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Back_Repuestos.Services
{
    public class CategoriaServicio
    {

            private readonly AppDbContext _context;

            public CategoriaServicio(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<object>> ObtenerCategoriasAsync()
            {
                return await _context.Categorias
                    .Select(c => new { c.IdCategoria, c.Nombre })
                    .ToListAsync();
            }

            public async Task<Categoria> CrearCategoriaAsync(Categoria categoria)
            {
                // Verificar duplicados
                if (await _context.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre))
                    throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }

            public async Task<Categoria> EditarCategoriaAsync(int id, Categoria categoria)
            {
                var cat = await _context.Categorias.FindAsync(id);
                if (cat == null)
                    throw new KeyNotFoundException("Categoría no encontrada.");

                cat.Nombre = categoria.Nombre;
                await _context.SaveChangesAsync();
                return cat;
            }

            public async Task<string> EliminarCategoriaAsync(int id)
            {
                var cat = await _context.Categorias.FindAsync(id);
                if (cat == null)
                    throw new KeyNotFoundException("Categoría no encontrada.");

                _context.Categorias.Remove(cat);
                await _context.SaveChangesAsync();
                return $"Categoría '{cat.Nombre}' eliminada correctamente.";
            }
    }
}

