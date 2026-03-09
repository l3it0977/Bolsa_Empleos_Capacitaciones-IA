namespace BolsaEmpleos.Application.DTOs.IA;

// DTO con el feedback detallado de la IA para una pregunta especifica.
public class FeedbackPreguntaDto
{
    // Numero de la pregunta evaluada
    public int NumeroPregunta { get; set; }

    // Indica si la respuesta del joven fue correcta
    public bool EsCorrecta { get; set; }

    // Explicacion de la respuesta correcta proporcionada por la IA
    public string Explicacion { get; set; } = string.Empty;
}
