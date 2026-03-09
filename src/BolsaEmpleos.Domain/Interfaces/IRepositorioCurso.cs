using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Curso.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioCurso : IRepositorio<Curso>
{
    // Obtiene todos los cursos que ensenan una habilidad especifica
    Task<IEnumerable<Curso>> ObtenerPorHabilidadAsync(int habilidadId);

    // Obtiene un curso junto con sus evaluaciones registradas
    Task<Curso?> ObtenerConEvaluacionesAsync(int cursoId);
}
