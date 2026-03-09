using BolsaEmpleos.Application.DTOs.IA;

namespace BolsaEmpleos.Application.Interfaces;

// Abstraccion del cliente de inteligencia artificial.
// Permite desacoplar la logica de negocio de cualquier proveedor de IA concreto.
// La implementacion de referencia usa NotebookLM como motor de IA.
public interface IClienteIA
{
    // Genera preguntas de evaluacion a partir del contenido de un curso.
    // Retorna una lista de preguntas con sus opciones de respuesta.
    Task<List<PreguntaIADto>> GenerarPreguntasAsync(
        string tituloCurso,
        string descripcionCurso,
        int numeroPreguntas = 5);

    // Evalua las respuestas del joven comparandolas con el contenido del curso.
    // Retorna el puntaje (0-100), retroalimentacion general y feedback por pregunta.
    Task<(int puntaje, string retroalimentacion, List<FeedbackPreguntaDto> feedback)> EvaluarRespuestasAsync(
        string tituloCurso,
        List<PreguntaIADto> preguntas,
        List<RespuestaIADto> respuestas);
}
