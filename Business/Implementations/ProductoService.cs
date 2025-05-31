using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Utilities.Exceptions;

namespace Business.Implementations
{
    /// <summary>
    /// Implementación de los servicios de producto
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProductoService(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            try
            {
                return await _productoRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los productos", ex);
            }
        }

        public async Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(int categoriaId)
        {
            try
            {
                if (categoriaId <= 0)
                    throw new BusinessException("El ID de la categoría debe ser mayor que 0");

                return await _productoRepository.GetByCategoriaAsync(categoriaId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener los productos de la categoría {categoriaId}", ex);
            }
        }

        public async Task<ProductoDto> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID del producto debe ser mayor que 0");

                var producto = await _productoRepository.GetByIdAsync(id);
                if (producto == null)
                    throw new BusinessException($"No se encontró el producto con ID {id}");

                return producto;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el producto con ID {id}", ex);
            }
        }

        public async Task<ProductoDto> CreateAsync(CrearProductoDto productoDto)
        {
            try
            {
                // Validaciones de negocio
                await ValidarProductoDto(productoDto, null);

                return await _productoRepository.CreateAsync(productoDto);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el producto", ex);
            }
        }

        public async Task<ProductoDto> UpdateAsync(int id, ActualizarProductoDto productoDto)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID del producto debe ser mayor que 0");

                // Validaciones de negocio
                await ValidarProductoDto(productoDto, id);

                var productoActualizado = await _productoRepository.UpdateAsync(id, productoDto);
                if (productoActualizado == null)
                    throw new BusinessException($"No se encontró el producto con ID {id}");

                return productoActualizado;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar el producto con ID {id}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID del producto debe ser mayor que 0");

                var resultado = await _productoRepository.DeleteAsync(id);
                if (!resultado)
                    throw new BusinessException($"No se encontró el producto con ID {id}");

                return resultado;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el producto con ID {id}", ex);
            }
        }

        public async Task<bool> DeleteWithAuditAsync(int id, EliminacionLogicaDto eliminacionDto)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID del producto debe ser mayor que 0");

                if (string.IsNullOrWhiteSpace(eliminacionDto.UsuarioEliminacion))
                    throw new BusinessException("El usuario que realiza la eliminación es obligatorio");

                var resultado = await _productoRepository.DeleteWithAuditAsync(id, eliminacionDto);
                if (!resultado)
                    throw new BusinessException($"No se encontró el producto con ID {id}");

                return resultado;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el producto con ID {id}", ex);
            }
        }

        public async Task<ProductoDto> UpdatePartialAsync(int id, ActualizarParcialProductoDto productoDto)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID del producto debe ser mayor que 0");

                // Validaciones específicas para actualización parcial
                if (productoDto.Precio.HasValue && productoDto.Precio <= 0)
                    throw new BusinessException("El precio del producto debe ser mayor que 0");

                if (productoDto.Stock.HasValue && productoDto.Stock < 0)
                    throw new BusinessException("El stock del producto no puede ser negativo");

                if (!string.IsNullOrEmpty(productoDto.Nombre) && string.IsNullOrWhiteSpace(productoDto.Nombre))
                    throw new BusinessException("El nombre del producto no puede estar vacío");

                // Verificar si la categoría existe (si se está cambiando)
                if (productoDto.CategoriaId.HasValue)
                {
                    var categoriaExiste = await _categoriaRepository.GetByIdAsync(productoDto.CategoriaId.Value);
                    if (categoriaExiste == null)
                        throw new BusinessException($"No existe la categoría con ID {productoDto.CategoriaId.Value}");
                }

                var productoActualizado = await _productoRepository.UpdatePartialAsync(id, productoDto);
                if (productoActualizado == null)
                    throw new BusinessException($"No se encontró el producto con ID {id}");

                return productoActualizado;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar parcialmente el producto con ID {id}", ex);
            }
        }

        public async Task<IEnumerable<ProductoDto>> SearchAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllAsync();

                return await _productoRepository.SearchAsync(searchTerm);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al buscar productos con el término '{searchTerm}'", ex);
            }
        }

        private async Task ValidarProductoDto<T>(T productoDto, int? excludeId = null) where T : class
        {
            string nombre = string.Empty;
            string codigo = string.Empty;
            decimal precio = 0;
            int stock = 0;
            int categoriaId = 0;

            // Usar reflection para obtener las propiedades según el tipo
            var nombreProp = typeof(T).GetProperty("Nombre");
            var codigoProp = typeof(T).GetProperty("Codigo");
            var precioProp = typeof(T).GetProperty("Precio");
            var stockProp = typeof(T).GetProperty("Stock");
            var categoriaIdProp = typeof(T).GetProperty("CategoriaId");

            if (nombreProp != null) nombre = nombreProp.GetValue(productoDto)?.ToString() ?? string.Empty;
            if (codigoProp != null) codigo = codigoProp.GetValue(productoDto)?.ToString() ?? string.Empty;
            if (precioProp != null) precio = (decimal)(precioProp.GetValue(productoDto) ?? 0);
            if (stockProp != null) stock = (int)(stockProp.GetValue(productoDto) ?? 0);
            if (categoriaIdProp != null) categoriaId = (int)(categoriaIdProp.GetValue(productoDto) ?? 0);

            // Validaciones
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BusinessException("El nombre del producto es obligatorio");

            if (string.IsNullOrWhiteSpace(codigo))
                throw new BusinessException("El código del producto es obligatorio");

            if (precio <= 0)
                throw new BusinessException("El precio del producto debe ser mayor que 0");

            if (stock < 0)
                throw new BusinessException("El stock del producto no puede ser negativo");

            if (categoriaId <= 0)
                throw new BusinessException("Debe seleccionar una categoría válida");

            // Verificar si la categoría existe
            var categoriaExiste = await _categoriaRepository.GetByIdAsync(categoriaId);
            if (categoriaExiste == null)
                throw new BusinessException($"No existe la categoría con ID {categoriaId}");

            // Verificar si ya existe un producto con el mismo código
            if (await _productoRepository.ExistsByCodigoAsync(codigo, excludeId))
                throw new BusinessException($"Ya existe un producto con el código '{codigo}'");
        }
    }
}
