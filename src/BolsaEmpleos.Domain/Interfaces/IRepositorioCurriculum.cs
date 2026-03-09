using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Curriculum.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioCurriculum : IRepositorio<Curriculum>
{
    // Obtiene el curriculum de un joven especifico junto con sus habilidades
    Task<Curriculum?> ObtenerPorJovenAsync(int jovenId);

    // Agrega una habilidad al curriculum de un joven
    Task AgregarHabilidadAsync(int curriculumId, int habilidadId, bool obtenidaPorCurso);

    // Verifica si el curriculum ya contiene una habilidad especifica
    Task<bool> TieneHabilidadAsync(int curriculumId, int habilidadId);
}
