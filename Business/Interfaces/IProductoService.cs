using Entity.DTOs;

namespace Business.Interfaces
{
    /// <summary>
    /// Interfaz para los servicios de producto
    /// </summary>
    public interface IProductoService
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
        /// <returns>Producto encontrado</returns>
        Task<ProductoDto> GetByIdAsync(int id);

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
        Task<ProductoDto> UpdateAsync(int id, ActualizarProductoDto productoDto);

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Busca productos por nombre o código
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden</returns>
        Task<IEnumerable<ProductoDto>> SearchAsync(string searchTerm);

        /// <summary>
        /// Elimina un producto con campos de auditoría
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="eliminacionDto">Datos de auditoría para la eliminación</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteWithAuditAsync(int id, EliminacionLogicaDto eliminacionDto);

        /// <summary>
        /// Actualiza parcialmente un producto
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="productoDto">Campos a actualizar</param>
        /// <returns>Producto actualizado</returns>
        Task<ProductoDto> UpdatePartialAsync(int id, ActualizarParcialProductoDto productoDto);
    }
}
