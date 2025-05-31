using Entity.DTOs;

namespace Business.Interfaces
{
    /// <summary>
    /// Interfaz para los servicios de categoría
    /// </summary>
    public interface ICategoriaService
    {
        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        /// <returns>Lista de categorías</returns>
        Task<IEnumerable<CategoriaDto>> GetAllAsync();

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría encontrada</returns>
        Task<CategoriaDto> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una categoría con sus productos
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría con productos</returns>
        Task<CategoriaConProductosDto> GetByIdWithProductsAsync(int id);

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        /// <param name="categoriaDto">Datos de la nueva categoría</param>
        /// <returns>Categoría creada</returns>
        Task<CategoriaDto> CreateAsync(CrearCategoriaDto categoriaDto);

        /// <summary>
        /// Actualiza una categoría existente
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <param name="categoriaDto">Datos actualizados</param>
        /// <returns>Categoría actualizada</returns>
        Task<CategoriaDto> UpdateAsync(int id, ActualizarCategoriaDto categoriaDto);

        /// <summary>
        /// Elimina una categoría
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);
    }
}
