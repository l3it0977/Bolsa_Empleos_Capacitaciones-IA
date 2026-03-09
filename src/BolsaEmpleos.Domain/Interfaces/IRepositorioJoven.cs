using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Joven.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioJoven : IRepositorio<Joven>
{
    // Busca un joven por su correo electronico (usado en autenticacion)
    Task<Joven?> ObtenerPorCorreoAsync(string correoElectronico);

    // Obtiene un joven junto con su curriculum y habilidades
    Task<Joven?> ObtenerConCurriculumAsync(int jovenId);

    // Obtiene un joven junto con todas sus evaluaciones de cursos
    Task<Joven?> ObtenerConEvaluacionesAsync(int jovenId);
}
