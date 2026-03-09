using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.IA;

// DTO con las respuestas del joven para que la IA las evalúe.
public class SolicitudEvaluarRespuestasDto
{
    // Identificador de la evaluacion activa del joven
    [Required(ErrorMessage = "El identificador de la evaluacion es obligatorio.")]
    public int EvaluacionId { get; set; }

    // Preguntas originales que la IA genero para el joven (deben ser las mismas que se presentaron)
    [Required(ErrorMessage = "Debe proporcionar las preguntas originales.")]
    [MinLength(1, ErrorMessage = "Debe proporcionar al menos una pregunta.")]
    public List<PreguntaIADto> Preguntas { get; set; } = new();

    // Lista de respuestas proporcionadas por el joven a cada pregunta
    [Required(ErrorMessage = "Debe proporcionar las respuestas.")]
    [MinLength(1, ErrorMessage = "Debe proporcionar al menos una respuesta.")]
    public List<RespuestaIADto> Respuestas { get; set; } = new();
}
