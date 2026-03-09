using BolsaEmpleos.Application.DTOs.Evaluacion;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de evaluaciones de cursos.
// Al aprobar una evaluacion, la habilidad del curso se agrega al curriculum del joven.
public interface IServicioEvaluacion
{
    // Obtiene todas las evaluaciones de un joven especifico
    Task<IEnumerable<EvaluacionDto>> ObtenerPorJovenAsync(int jovenId);

    // Obtiene una evaluacion por su identificador unico
    Task<EvaluacionDto?> ObtenerPorIdAsync(int id);

    // Inicia la evaluacion de un curso para un joven
    Task<EvaluacionDto> IniciarEvaluacionAsync(int jovenId, int cursoId);

    // Registra el resultado de una evaluacion y actualiza el curriculum si es aprobada
    Task<EvaluacionDto?> RegistrarResultadoAsync(int evaluacionId, RegistrarResultadoEvaluacionDto dto);
}
