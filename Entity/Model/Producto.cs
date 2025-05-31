using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    /// <summary>
    /// Entidad que representa un producto
    /// </summary>
    [Table("Productos")]
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del producto
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto
        /// </summary>
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Precio del producto
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        /// <summary>
        /// Cantidad en stock
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// Código único del producto
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el producto está activo
        /// </summary>
        public bool Estado { get; set; } = true;

        /// <summary>
        /// Fecha de creación del producto
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Fecha de eliminación lógica
        /// </summary>
        public DateTime? FechaEliminacion { get; set; }

        /// <summary>
        /// Usuario que eliminó el registro
        /// </summary>
        [StringLength(100)]
        public string? UsuarioEliminacion { get; set; }

        /// <summary>
        /// Motivo de la eliminación
        /// </summary>
        [StringLength(500)]
        public string? MotivoEliminacion { get; set; }

        /// <summary>
        /// Clave foránea hacia la categoría
        /// </summary>
        [Required]
        public int CategoriaId { get; set; }

        /// <summary>
        /// Navegación hacia la categoría padre
        /// </summary>
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; } = null!;
    }
}
