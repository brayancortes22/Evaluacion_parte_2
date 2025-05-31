using Entity.Contexts;
using Entity.DTOs;
using Entity.Model;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementations
{
    /// <summary>
    /// Implementación del repositorio de productos
    /// </summary>
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
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
                    CategoriaNombre = p.Categoria.Nombre
                })
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId && p.Estado)
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
                    CategoriaNombre = p.Categoria.Nombre
                })
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<ProductoDto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Id == id && p.Estado)
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
                    CategoriaNombre = p.Categoria.Nombre
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProductoDto> CreateAsync(CrearProductoDto productoDto)
        {
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Precio = productoDto.Precio,
                Stock = productoDto.Stock,
                Codigo = productoDto.Codigo,
                Estado = productoDto.Estado,
                CategoriaId = productoDto.CategoriaId,
                FechaCreacion = DateTime.Now
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            // Cargar la categoría para devolver el DTO completo
            await _context.Entry(producto)
                .Reference(p => p.Categoria)
                .LoadAsync();

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Codigo = producto.Codigo,
                Estado = producto.Estado,
                FechaCreacion = producto.FechaCreacion,
                FechaModificacion = producto.FechaModificacion,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria.Nombre
            };
        }

        public async Task<ProductoDto?> UpdateAsync(int id, ActualizarProductoDto productoDto)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && p.Estado);

            if (producto == null)
                return null;

            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.Precio = productoDto.Precio;
            producto.Stock = productoDto.Stock;
            producto.Codigo = productoDto.Codigo;
            producto.Estado = productoDto.Estado;
            producto.CategoriaId = productoDto.CategoriaId;
            producto.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();

            // Recargar la categoría si cambió
            if (producto.CategoriaId != productoDto.CategoriaId)
            {
                await _context.Entry(producto)
                    .Reference(p => p.Categoria)
                    .LoadAsync();
            }

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Codigo = producto.Codigo,
                Estado = producto.Estado,
                FechaCreacion = producto.FechaCreacion,
                FechaModificacion = producto.FechaModificacion,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria.Nombre
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id && p.Estado);

            if (producto == null)
                return false;

            producto.Estado = false;
            producto.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
        {
            var query = _context.Productos.Where(p => p.Codigo.ToLower() == codigo.ToLower() && p.Estado);
            
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<ProductoDto>> SearchAsync(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Estado && 
                    (p.Nombre.ToLower().Contains(lowerSearchTerm) || 
                     p.Codigo.ToLower().Contains(lowerSearchTerm)))
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
                    CategoriaNombre = p.Categoria.Nombre
                })
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
    }
}
