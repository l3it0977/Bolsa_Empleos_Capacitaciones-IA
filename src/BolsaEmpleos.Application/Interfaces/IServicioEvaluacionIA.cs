using BolsaEmpleos.Application.DTOs.IA;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de evaluacion con soporte de inteligencia artificial.
// Coordina la generacion de preguntas con NotebookLM, la evaluacion de respuestas y
// la notificacion al backend para agregar la habilidad al CV si el joven aprueba.
public interface IServicioEvaluacionIA
{
    // Genera las preguntas de evaluacion para un curso usando la IA.
    // El joven debe tener una evaluacion activa (en curso) para el curso indicado.
    Task<List<PreguntaIADto>> GenerarPreguntasAsync(int cursoId, int numeroPreguntas = 5);

    // Evalua las respuestas del joven usando la IA.
    // Si el puntaje supera el minimo, notifica al backend para agregar la habilidad al CV.
    Task<ResultadoEvaluacionIADto> EvaluarRespuestasAsync(SolicitudEvaluarRespuestasDto solicitud);
}
