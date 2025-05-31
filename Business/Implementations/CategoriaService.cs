using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Utilities.Exceptions;

namespace Business.Implementations
{
    /// <summary>
    /// Implementación de los servicios de categoría
    /// </summary>
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            try
            {
                return await _categoriaRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener las categorías", ex);
            }
        }

        public async Task<CategoriaDto> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID de la categoría debe ser mayor que 0");

                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null)
                    throw new BusinessException($"No se encontró la categoría con ID {id}");

                return categoria;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la categoría con ID {id}", ex);
            }
        }

        public async Task<CategoriaConProductosDto> GetByIdWithProductsAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID de la categoría debe ser mayor que 0");

                var categoria = await _categoriaRepository.GetByIdWithProductsAsync(id);
                if (categoria == null)
                    throw new BusinessException($"No se encontró la categoría con ID {id}");

                return categoria;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la categoría con productos para ID {id}", ex);
            }
        }

        public async Task<CategoriaDto> CreateAsync(CrearCategoriaDto categoriaDto)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(categoriaDto.Nombre))
                    throw new BusinessException("El nombre de la categoría es obligatorio");

                // Verificar si ya existe una categoría con el mismo nombre
                if (await _categoriaRepository.ExistsAsync(categoriaDto.Nombre))
                    throw new BusinessException($"Ya existe una categoría con el nombre '{categoriaDto.Nombre}'");

                return await _categoriaRepository.CreateAsync(categoriaDto);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear la categoría", ex);
            }
        }

        public async Task<CategoriaDto> UpdateAsync(int id, ActualizarCategoriaDto categoriaDto)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID de la categoría debe ser mayor que 0");

                if (string.IsNullOrWhiteSpace(categoriaDto.Nombre))
                    throw new BusinessException("El nombre de la categoría es obligatorio");

                // Verificar si ya existe una categoría con el mismo nombre (excluyendo la actual)
                if (await _categoriaRepository.ExistsAsync(categoriaDto.Nombre, id))
                    throw new BusinessException($"Ya existe otra categoría con el nombre '{categoriaDto.Nombre}'");

                var categoriaActualizada = await _categoriaRepository.UpdateAsync(id, categoriaDto);
                if (categoriaActualizada == null)
                    throw new BusinessException($"No se encontró la categoría con ID {id}");

                return categoriaActualizada;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar la categoría con ID {id}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new BusinessException("El ID de la categoría debe ser mayor que 0");

                var resultado = await _categoriaRepository.DeleteAsync(id);
                if (!resultado)
                    throw new BusinessException("No se puede eliminar la categoría. Puede que no exista o tenga productos asociados");

                return resultado;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar la categoría con ID {id}", ex);
            }
        }
    }
}
