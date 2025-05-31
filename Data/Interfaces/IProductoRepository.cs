using Entity.DTOs;

namespace Data.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de productos
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        /// <returns>Lista de productos</returns>
        Task<IEnumerable<ProductoDto>> GetAllAsync();

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns>Lista de productos de la categoría</returns>
        Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o null</returns>
        Task<ProductoDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productoDto">Datos del nuevo producto</param>
        /// <returns>Producto creado</returns>
        Task<ProductoDto> CreateAsync(CrearProductoDto productoDto);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="productoDto">Datos actualizados</param>
        /// <returns>Producto actualizado</returns>
        Task<ProductoDto?> UpdateAsync(int id, ActualizarProductoDto productoDto);

        /// <summary>
        /// Elimina un producto (soft delete)
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Verifica si existe un producto con el código especificado
        /// </summary>
        /// <param name="codigo">Código del producto</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <returns>True si existe</returns>
        Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null);

        /// <summary>
        /// Busca productos por nombre o código
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden</returns>
        Task<IEnumerable<ProductoDto>> SearchAsync(string searchTerm);
    }
}
