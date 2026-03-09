namespace BolsaEmpleos.Application.DTOs.IA;

// DTO con el resultado completo de la evaluacion realizada por la inteligencia artificial.
// Incluye el puntaje, si el joven aprobo, la retroalimentacion y la habilidad agregada al CV.
public class ResultadoEvaluacionIADto
{
    // Identificador de la evaluacion evaluada
    public int EvaluacionId { get; set; }

    // Puntaje obtenido por el joven sobre 100
    public int PuntajeObtenido { get; set; }

    // Puntaje minimo requerido para aprobar el curso
    public int PuntajeMinimo { get; set; }

    // Indica si el joven aprobo la evaluacion
    public bool Aprobo { get; set; }

    // Retroalimentacion general proporcionada por la IA sobre el desempeno del joven
    public string Retroalimentacion { get; set; } = string.Empty;

    // Detalle del feedback por cada pregunta
    public List<FeedbackPreguntaDto> FeedbackPreguntas { get; set; } = new();

    // Nombre de la habilidad agregada al curriculum (solo si el joven aprobo)
    public string? HabilidadAgregada { get; set; }
}
