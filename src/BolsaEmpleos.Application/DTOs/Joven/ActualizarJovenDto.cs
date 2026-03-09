using System.ComponentModel.DataAnnotations;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.Joven;

// DTO utilizado para actualizar los datos de un joven existente.
public class ActualizarJovenDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El apellido no puede superar 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del telefono no es valido.")]
    public string? Telefono { get; set; }

    public NivelEducativo NivelEducativo { get; set; }
}
