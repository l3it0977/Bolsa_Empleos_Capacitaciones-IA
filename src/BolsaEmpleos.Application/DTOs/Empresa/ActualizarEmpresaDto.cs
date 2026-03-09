using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Empresa;

// DTO utilizado para actualizar los datos de una empresa existente.
public class ActualizarEmpresaDto
{
    [Required(ErrorMessage = "La razon social es obligatoria.")]
    [MaxLength(200, ErrorMessage = "La razon social no puede superar 200 caracteres.")]
    public string RazonSocial { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del telefono no es valido.")]
    public string? Telefono { get; set; }

    [MaxLength(500, ErrorMessage = "La descripcion no puede superar 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Url(ErrorMessage = "El sitio web no tiene un formato de URL valido.")]
    public string? SitioWeb { get; set; }
}
