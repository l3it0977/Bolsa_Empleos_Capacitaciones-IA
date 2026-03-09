namespace BolsaEmpleos.Application.DTOs.IA;

// DTO que representa una pregunta generada por la inteligencia artificial
// a partir del contenido del curso.
public class PreguntaIADto
{
    // Numero de orden de la pregunta en la evaluacion
    public int Numero { get; set; }

    // Enunciado o texto de la pregunta
    public string Enunciado { get; set; } = string.Empty;

    // Tipo de pregunta: "opcion_multiple", "verdadero_falso" o "abierta"
    public string Tipo { get; set; } = "opcion_multiple";

    // Opciones de respuesta para preguntas de multiple opcion o verdadero/falso
    public List<string> Opciones { get; set; } = new();
}
