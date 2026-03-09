using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Habilidad;

// DTO utilizado para crear o actualizar una habilidad en el sistema.
public class GuardarHabilidadDto
{
    [Required(ErrorMessage = "El nombre de la habilidad es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La descripcion no puede superar 500 caracteres.")]
    public string? Descripcion { get; set; }

    [MaxLength(100, ErrorMessage = "La categoria no puede superar 100 caracteres.")]
    public string? Categoria { get; set; }
}
