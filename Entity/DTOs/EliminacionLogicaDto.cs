using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    /// <summary>
    /// DTO para eliminación lógica con campos de auditoría
    /// </summary>
    public class EliminacionLogicaDto
    {
        /// <summary>
        /// Usuario que realiza la eliminación
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UsuarioEliminacion { get; set; } = string.Empty;

        /// <summary>
        /// Motivo de la eliminación
        /// </summary>
        [StringLength(500)]
        public string? MotivoEliminacion { get; set; }
    }
}
