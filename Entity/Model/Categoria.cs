using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    /// <summary>
    /// Entidad que representa una categoría de productos
    /// </summary>
    [Table("Categorias")]
    public class Categoria
    {
        /// <summary>
        /// Identificador único de la categoría
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la categoría
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Indica si la categoría está activa
        /// </summary>
        public bool Estado { get; set; } = true;

        /// <summary>
        /// Fecha de creación de la categoría
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Navegación hacia los productos de esta categoría
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
