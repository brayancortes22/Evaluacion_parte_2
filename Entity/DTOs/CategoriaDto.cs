using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    /// <summary>
    /// DTO para consultar información de una categoría
    /// </summary>
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int TotalProductos { get; set; }
    }

    /// <summary>
    /// DTO para crear una nueva categoría
    /// </summary>
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;
    }

    /// <summary>
    /// DTO para actualizar una categoría existente
    /// </summary>
    public class ActualizarCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }

    /// <summary>
    /// DTO para consultar categorías con sus productos
    /// </summary>
    public class CategoriaConProductosDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<ProductoDto> Productos { get; set; } = new List<ProductoDto>();
    }
}
