using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Curso;

// DTO utilizado para crear o actualizar un curso de capacitacion.
public class GuardarCursoDto
{
    [Required(ErrorMessage = "El titulo del curso es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripcion del curso es obligatoria.")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La habilidad del curso es obligatoria.")]
    public int HabilidadId { get; set; }

    // Duracion en horas; debe estar entre 1 y 3 horas segun las reglas del negocio
    [Range(1, 3, ErrorMessage = "La duracion del curso debe estar entre 1 y 3 horas.")]
    public decimal DuracionHoras { get; set; }

    [Url(ErrorMessage = "La URL del material no tiene un formato valido.")]
    public string? UrlMaterial { get; set; }

    // Puntaje minimo requerido para aprobar (entre 0 y 100)
    [Range(0, 100, ErrorMessage = "El puntaje minimo debe estar entre 0 y 100.")]
    public int PuntajeMinimoAprobacion { get; set; } = 70;
}
