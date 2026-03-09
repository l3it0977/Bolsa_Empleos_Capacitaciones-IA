using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Evaluacion.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioEvaluacion : IRepositorio<Evaluacion>
{
    // Obtiene todas las evaluaciones de un joven especifico
    Task<IEnumerable<Evaluacion>> ObtenerPorJovenAsync(int jovenId);

    // Obtiene la evaluacion de un joven para un curso especifico
    Task<Evaluacion?> ObtenerPorJovenYCursoAsync(int jovenId, int cursoId);

    // Obtiene todas las evaluaciones aprobadas de un joven
    Task<IEnumerable<Evaluacion>> ObtenerAprobadasPorJovenAsync(int jovenId);

    // Obtiene evaluaciones filtradas por estado
    Task<IEnumerable<Evaluacion>> ObtenerPorEstadoAsync(EstadoEvaluacion estado);
}
