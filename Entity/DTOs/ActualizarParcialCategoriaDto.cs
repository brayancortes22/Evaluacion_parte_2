using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    /// <summary>
    /// DTO para actualización parcial de categoría
    /// </summary>
    public class ActualizarParcialCategoriaDto
    {
        /// <summary>
        /// Nombre de la categoría
        /// </summary>
        [StringLength(100)]
        public string? Nombre { get; set; }

        /// <summary>
        /// Descripción de la categoría
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Estado de la categoría
        /// </summary>
        public bool? Estado { get; set; }
    }
}
