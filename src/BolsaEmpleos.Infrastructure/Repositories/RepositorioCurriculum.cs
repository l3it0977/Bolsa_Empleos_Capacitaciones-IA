using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de curricula.
// Extiende el repositorio generico con consultas y operaciones especificas del negocio.
public class RepositorioCurriculum : Repositorio<Curriculum>, IRepositorioCurriculum
{
    public RepositorioCurriculum(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Obtiene el curriculum de un joven con todas sus habilidades
    public async Task<Curriculum?> ObtenerPorJovenAsync(int jovenId)
    {
        return await _conjunto
            .Include(c => c.Joven)
            .Include(c => c.CurriculumHabilidades.Where(ch => ch.Activo))
                .ThenInclude(ch => ch.Habilidad)
            .FirstOrDefaultAsync(c => c.JovenId == jovenId && c.Activo);
    }

    // Agrega una nueva habilidad al curriculum y persiste los cambios
    public async Task AgregarHabilidadAsync(int curriculumId, int habilidadId, bool obtenidaPorCurso)
    {
        var curriculumHabilidad = new CurriculumHabilidad
        {
            CurriculumId = curriculumId,
            HabilidadId = habilidadId,
            ObtenidaPorCurso = obtenidaPorCurso,
            FechaAgregado = DateTime.UtcNow
        };

        await _contexto.CurriculumHabilidades.AddAsync(curriculumHabilidad);
        await _contexto.SaveChangesAsync();
    }

    // Verifica si el curriculum ya contiene una habilidad especifica
    public async Task<bool> TieneHabilidadAsync(int curriculumId, int habilidadId)
    {
        return await _contexto.CurriculumHabilidades
            .AnyAsync(ch =>
                ch.CurriculumId == curriculumId &&
                ch.HabilidadId == habilidadId &&
                ch.Activo);
    }
}
