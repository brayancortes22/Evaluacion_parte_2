using Entity.Contexts;
using Entity.DTOs;
using Entity.Model;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementations
{
    /// <summary>
    /// Implementación del repositorio de categorías
    /// </summary>
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            return await _context.Categorias
                .Where(c => c.Estado)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado,
                    FechaCreacion = c.FechaCreacion,
                    FechaModificacion = c.FechaModificacion,
                    TotalProductos = c.Productos.Count(p => p.Estado)
                })
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            return await _context.Categorias
                .Where(c => c.Id == id && c.Estado)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado,
                    FechaCreacion = c.FechaCreacion,
                    FechaModificacion = c.FechaModificacion,
                    TotalProductos = c.Productos.Count(p => p.Estado)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoriaConProductosDto?> GetByIdWithProductsAsync(int id)
        {
            return await _context.Categorias
                .Where(c => c.Id == id && c.Estado)
                .Select(c => new CategoriaConProductosDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado,
                    FechaCreacion = c.FechaCreacion,
                    Productos = c.Productos
                        .Where(p => p.Estado)
                        .Select(p => new ProductoDto
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Codigo = p.Codigo,
                            Estado = p.Estado,
                            FechaCreacion = p.FechaCreacion,
                            FechaModificacion = p.FechaModificacion,
                            CategoriaId = p.CategoriaId,
                            CategoriaNombre = c.Nombre
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoriaDto> CreateAsync(CrearCategoriaDto categoriaDto)
        {
            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion,
                Estado = categoriaDto.Estado,
                FechaCreacion = DateTime.Now
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion,
                FechaModificacion = categoria.FechaModificacion,
                TotalProductos = 0
            };
        }

        public async Task<CategoriaDto?> UpdateAsync(int id, ActualizarCategoriaDto categoriaDto)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.Estado);

            if (categoria == null)
                return null;

            categoria.Nombre = categoriaDto.Nombre;
            categoria.Descripcion = categoriaDto.Descripcion;
            categoria.Estado = categoriaDto.Estado;
            categoria.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion,
                FechaModificacion = categoria.FechaModificacion,
                TotalProductos = await _context.Productos.CountAsync(p => p.CategoriaId == categoria.Id && p.Estado)
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.Estado);

            if (categoria == null)
                return false;

            // Verificar si tiene productos activos
            var tieneProductosActivos = await _context.Productos
                .AnyAsync(p => p.CategoriaId == id && p.Estado);

            if (tieneProductosActivos)
                return false; // No se puede eliminar si tiene productos activos

            categoria.Estado = false;
            categoria.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWithAuditAsync(int id, EliminacionLogicaDto eliminacionDto)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Productos.Where(p => p.Estado))
                .FirstOrDefaultAsync(c => c.Id == id && c.Estado);

            if (categoria == null)
                return false;

            // Verificar si tiene productos activos
            bool tieneProductosActivos = categoria.Productos.Any();
            if (tieneProductosActivos)
                return false; // No se puede eliminar si tiene productos activos

            categoria.Estado = false;
            categoria.FechaModificacion = DateTime.Now;
            categoria.FechaEliminacion = DateTime.Now;
            categoria.UsuarioEliminacion = eliminacionDto.UsuarioEliminacion;
            categoria.MotivoEliminacion = eliminacionDto.MotivoEliminacion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CategoriaDto?> UpdatePartialAsync(int id, ActualizarParcialCategoriaDto categoriaDto)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.Estado);
            if (categoria == null)
                return null;

            // Actualizar solo los campos que vienen con valores
            if (!string.IsNullOrEmpty(categoriaDto.Nombre))
            {
                categoria.Nombre = categoriaDto.Nombre;
            }

            if (categoriaDto.Descripcion != null)
            {
                categoria.Descripcion = categoriaDto.Descripcion;
            }

            if (categoriaDto.Estado.HasValue)
            {
                categoria.Estado = categoriaDto.Estado.Value;
            }

            categoria.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion,
                FechaModificacion = categoria.FechaModificacion
            };
        }

        public async Task<bool> ExistsAsync(string nombre, int? excludeId = null)
        {
            var query = _context.Categorias.Where(c => c.Nombre.ToLower() == nombre.ToLower() && c.Estado);
            
            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
