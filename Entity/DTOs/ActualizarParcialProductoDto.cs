using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    /// <summary>
    /// DTO para actualización parcial de producto
    /// </summary>
    public class ActualizarParcialProductoDto
    {
        /// <summary>
        /// Nombre del producto
        /// </summary>
        [StringLength(150)]
        public string? Nombre { get; set; }

        /// <summary>
        /// Descripción del producto
        /// </summary>
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Precio del producto
        /// </summary>
        public decimal? Precio { get; set; }

        /// <summary>
        /// Cantidad en stock
        /// </summary>
        public int? Stock { get; set; }

        /// <summary>
        /// Estado del producto
        /// </summary>
        public bool? Estado { get; set; }

        /// <summary>
        /// ID de la categoría
        /// </summary>
        public int? CategoriaId { get; set; }
    }
}
