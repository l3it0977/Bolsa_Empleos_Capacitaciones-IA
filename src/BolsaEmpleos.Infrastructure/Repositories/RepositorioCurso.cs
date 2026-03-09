using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de cursos de capacitacion.
public class RepositorioCurso : Repositorio<Curso>, IRepositorioCurso
{
    public RepositorioCurso(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Obtiene los cursos activos que ensenan una habilidad especifica
    public async Task<IEnumerable<Curso>> ObtenerPorHabilidadAsync(int habilidadId)
    {
        return await _conjunto
            .Include(c => c.Habilidad)
            .Where(c => c.HabilidadId == habilidadId && c.Activo)
            .ToListAsync();
    }

    // Obtiene un curso con todas sus evaluaciones asociadas
    public async Task<Curso?> ObtenerConEvaluacionesAsync(int cursoId)
    {
        return await _conjunto
            .Include(c => c.Habilidad)
            .Include(c => c.Evaluaciones.Where(e => e.Activo))
                .ThenInclude(e => e.Joven)
            .FirstOrDefaultAsync(c => c.Id == cursoId && c.Activo);
    }

    // Sobreescribe ObtenerPorIdAsync para incluir la habilidad por defecto
    public override async Task<Curso?> ObtenerPorIdAsync(int id)
    {
        return await _conjunto
            .Include(c => c.Habilidad)
            .FirstOrDefaultAsync(c => c.Id == id && c.Activo);
    }
}
