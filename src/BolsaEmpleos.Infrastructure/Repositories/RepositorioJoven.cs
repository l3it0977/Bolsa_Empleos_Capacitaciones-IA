using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de jovenes.
// Extiende el repositorio generico con consultas especificas del negocio.
public class RepositorioJoven : Repositorio<Joven>, IRepositorioJoven
{
    public RepositorioJoven(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Busca un joven activo por su correo electronico
    public async Task<Joven?> ObtenerPorCorreoAsync(string correoElectronico)
    {
        return await _conjunto
            .FirstOrDefaultAsync(j =>
                j.CorreoElectronico == correoElectronico && j.Activo);
    }

    // Obtiene un joven con su curriculum y las habilidades del curriculum
    public async Task<Joven?> ObtenerConCurriculumAsync(int jovenId)
    {
        return await _conjunto
            .Include(j => j.Curriculum)
                .ThenInclude(c => c!.CurriculumHabilidades)
                    .ThenInclude(ch => ch.Habilidad)
            .FirstOrDefaultAsync(j => j.Id == jovenId && j.Activo);
    }

    // Obtiene un joven con todas sus evaluaciones y los cursos asociados
    public async Task<Joven?> ObtenerConEvaluacionesAsync(int jovenId)
    {
        return await _conjunto
            .Include(j => j.Evaluaciones)
                .ThenInclude(e => e.Curso)
            .FirstOrDefaultAsync(j => j.Id == jovenId && j.Activo);
    }
}
