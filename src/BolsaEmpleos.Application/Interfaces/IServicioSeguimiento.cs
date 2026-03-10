using BolsaEmpleos.Application.DTOs.Postulacion;
using BolsaEmpleos.Application.DTOs.Seguimiento;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de seguimiento laboral y recomendacion de empleos.
// Coordina el registro del resultado de empleo, el analisis del historial del joven
// y la recomendacion de nuevas ofertas compatibles mediante el algoritmo de parentesco.
public interface IServicioSeguimiento
{
    // Obtiene el resumen completo de seguimiento laboral de un joven:
    // historial de postulaciones, estado de empleo actual y, si no esta contratado,
    // una lista de ofertas recomendadas ordenadas por compatibilidad con su CV.
    Task<ResultadoSeguimientoDto> ObtenerSeguimientoAsync(int jovenId);

    // Aplica el algoritmo de parentesco para recomendar ofertas compatibles con el CV del joven.
    // Analiza las habilidades del joven, las compara con los requisitos de cada oferta publicada
    // y retorna las recomendaciones ordenadas de mayor a menor porcentaje de compatibilidad.
    // Excluye ofertas a las que el joven ya se postulo.
    Task<IEnumerable<RecomendacionOfertaDto>> RecomendarOfertasAsync(int jovenId);

    // Registra si el joven consiguio empleo a traves de una postulacion especifica.
    // Actualiza el estado de la postulacion a Aceptada (consiguioEmpleo = true)
    // o Rechazada (consiguioEmpleo = false).
    Task<PostulacionDto?> RegistrarResultadoEmpleoAsync(int postulacionId, bool consiguioEmpleo);
}
