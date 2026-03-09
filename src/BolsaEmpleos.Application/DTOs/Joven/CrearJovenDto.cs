using System.ComponentModel.DataAnnotations;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.Joven;

// DTO utilizado para el registro de un nuevo joven en la plataforma.
public class CrearJovenDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El apellido no puede superar 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electronico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido.")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contrasena es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contrasena debe tener al menos 8 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del telefono no es valido.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    public NivelEducativo NivelEducativo { get; set; } = NivelEducativo.Secundaria;
}
