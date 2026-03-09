using BolsaEmpleos.Application.DTOs.Curso;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de cursos de capacitacion.
// Los cursos son el mecanismo de cierre de brechas de habilidades en la plataforma.
public interface IServicioCurso
{
    // Obtiene todos los cursos activos disponibles en la plataforma
    Task<IEnumerable<CursoDto>> ObtenerTodosAsync();

    // Obtiene un curso por su identificador unico
    Task<CursoDto?> ObtenerPorIdAsync(int id);

    // Obtiene los cursos que ensenan una habilidad especifica
    Task<IEnumerable<CursoDto>> ObtenerPorHabilidadAsync(int habilidadId);

    // Crea un nuevo curso de capacitacion
    Task<CursoDto> CrearAsync(GuardarCursoDto dto);

    // Actualiza un curso existente
    Task<CursoDto?> ActualizarAsync(int id, GuardarCursoDto dto);

    // Elimina logicamente un curso del sistema
    Task<bool> EliminarAsync(int id);
}
