using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Postulacion.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioPostulacion : IRepositorio<Postulacion>
{
    // Obtiene todas las postulaciones activas de un joven especifico
    Task<IEnumerable<Postulacion>> ObtenerPorJovenAsync(int jovenId);

    // Obtiene todas las postulaciones activas de una oferta de trabajo especifica
    Task<IEnumerable<Postulacion>> ObtenerPorOfertaAsync(int ofertaTrabajoId);

    // Verifica si un joven ya postulo a una oferta de trabajo determinada
    Task<Postulacion?> ObtenerPorJovenYOfertaAsync(int jovenId, int ofertaTrabajoId);

    // Obtiene postulaciones filtradas por estado
    Task<IEnumerable<Postulacion>> ObtenerPorEstadoAsync(EstadoPostulacion estado);
}
