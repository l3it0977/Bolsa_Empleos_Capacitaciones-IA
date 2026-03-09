using BolsaEmpleos.Application.DTOs.Curriculum;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de curriculum vitae.
// Gestiona la creacion, actualizacion y consulta del CV de cada joven,
// incluyendo la agregacion de habilidades obtenidas por cursos aprobados.
public interface IServicioCurriculum
{
    // Obtiene el curriculum de un joven especifico con todas sus habilidades
    Task<CurriculumDto?> ObtenerPorJovenAsync(int jovenId);

    // Crea o actualiza el curriculum de un joven
    Task<CurriculumDto> GuardarAsync(int jovenId, GuardarCurriculumDto dto);

    // Agrega una habilidad al curriculum de un joven
    Task<bool> AgregarHabilidadAsync(int jovenId, int habilidadId, bool obtenidaPorCurso);
}
