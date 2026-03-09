using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Curriculum;

// DTO utilizado para crear o actualizar el curriculum de un joven.
public class GuardarCurriculumDto
{
    [MaxLength(1000, ErrorMessage = "El resumen profesional no puede superar 1000 caracteres.")]
    public string? ResumenProfesional { get; set; }

    [MaxLength(200, ErrorMessage = "El titulo profesional no puede superar 200 caracteres.")]
    public string? TituloProfesional { get; set; }

    [Url(ErrorMessage = "La URL del portfolio no tiene un formato valido.")]
    public string? UrlPortfolio { get; set; }
}
