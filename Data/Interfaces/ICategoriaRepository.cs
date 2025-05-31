using Entity.DTOs;

namespace Data.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de categorías
    /// </summary>
    public interface ICategoriaRepository
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
        /// <returns>Categoría encontrada o null</returns>
        Task<CategoriaDto?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una categoría con sus productos
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría con productos</returns>
        Task<CategoriaConProductosDto?> GetByIdWithProductsAsync(int id);

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
        Task<CategoriaDto?> UpdateAsync(int id, ActualizarCategoriaDto categoriaDto);        /// <summary>
        /// Elimina una categoría (soft delete)
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Elimina una categoría con campos de auditoría (soft delete mejorado)
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <param name="eliminacionDto">Datos de auditoría para la eliminación</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteWithAuditAsync(int id, EliminacionLogicaDto eliminacionDto);

        /// <summary>
        /// Actualiza parcialmente una categoría
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <param name="categoriaDto">Campos a actualizar</param>
        /// <returns>Categoría actualizada</returns>
        Task<CategoriaDto?> UpdatePartialAsync(int id, ActualizarParcialCategoriaDto categoriaDto);

        /// <summary>
        /// Verifica si existe una categoría con el nombre especificado
        /// </summary>
        /// <param name="nombre">Nombre de la categoría</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <returns>True si existe</returns>
        Task<bool> ExistsAsync(string nombre, int? excludeId = null);
    }
}
