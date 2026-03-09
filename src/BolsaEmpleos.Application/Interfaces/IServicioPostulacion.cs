using BolsaEmpleos.Application.DTOs.Postulacion;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de postulaciones.
// Implementa la logica de validacion de requisitos antes de permitir la postulacion.
public interface IServicioPostulacion
{
    // Evalua si un joven puede postularse a una oferta comparando su CV con los requisitos.
    // Retorna un resultado detallado con las brechas de habilidades y cursos sugeridos.
    Task<ResultadoEvaluacionPostulacionDto> EvaluarPostulacionAsync(int jovenId, int ofertaTrabajoId);

    // Registra la postulacion de un joven a una oferta si cumple con todos los requisitos.
    // Lanza InvalidOperationException si el joven no puede postularse.
    Task<PostulacionDto> PostularAsync(int jovenId, int ofertaTrabajoId);

    // Obtiene todas las postulaciones realizadas por un joven especifico
    Task<IEnumerable<PostulacionDto>> ObtenerPorJovenAsync(int jovenId);

    // Obtiene todas las postulaciones recibidas para una oferta de trabajo especifica
    Task<IEnumerable<PostulacionDto>> ObtenerPorOfertaAsync(int ofertaTrabajoId);

    // Obtiene una postulacion por su identificador unico
    Task<PostulacionDto?> ObtenerPorIdAsync(int id);
}
