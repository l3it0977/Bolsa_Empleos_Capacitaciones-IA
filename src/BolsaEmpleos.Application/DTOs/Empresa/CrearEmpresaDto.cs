using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Empresa;

// DTO utilizado para el registro de una nueva empresa en la plataforma.
public class CrearEmpresaDto
{
    [Required(ErrorMessage = "La razon social es obligatoria.")]
    [MaxLength(200, ErrorMessage = "La razon social no puede superar 200 caracteres.")]
    public string RazonSocial { get; set; } = string.Empty;

    [Required(ErrorMessage = "El numero de identificacion es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El numero de identificacion no puede superar 20 caracteres.")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electronico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido.")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contrasena es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contrasena debe tener al menos 8 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del telefono no es valido.")]
    public string? Telefono { get; set; }

    [MaxLength(500, ErrorMessage = "La descripcion no puede superar 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Url(ErrorMessage = "El sitio web no tiene un formato de URL valido.")]
    public string? SitioWeb { get; set; }
}
