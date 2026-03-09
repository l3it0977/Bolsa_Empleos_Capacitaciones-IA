using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de evaluaciones de cursos.
public class RepositorioEvaluacion : Repositorio<Evaluacion>, IRepositorioEvaluacion
{
    public RepositorioEvaluacion(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Obtiene todas las evaluaciones activas de un joven especifico
    public async Task<IEnumerable<Evaluacion>> ObtenerPorJovenAsync(int jovenId)
    {
        return await _conjunto
            .Include(e => e.Joven)
            .Include(e => e.Curso)
            .Where(e => e.JovenId == jovenId && e.Activo)
            .OrderByDescending(e => e.FechaCreacion)
            .ToListAsync();
    }

    // Obtiene la ultima evaluacion de un joven para un curso especifico
    public async Task<Evaluacion?> ObtenerPorJovenYCursoAsync(int jovenId, int cursoId)
    {
        return await _conjunto
            .Include(e => e.Joven)
            .Include(e => e.Curso)
            .Where(e => e.JovenId == jovenId && e.CursoId == cursoId && e.Activo)
            .OrderByDescending(e => e.FechaCreacion)
            .FirstOrDefaultAsync();
    }

    // Obtiene todas las evaluaciones aprobadas de un joven
    public async Task<IEnumerable<Evaluacion>> ObtenerAprobadaspPorJovenAsync(int jovenId)
    {
        return await _conjunto
            .Include(e => e.Curso)
                .ThenInclude(c => c.Habilidad)
            .Where(e => e.JovenId == jovenId &&
                        e.Estado == EstadoEvaluacion.Aprobada &&
                        e.Activo)
            .ToListAsync();
    }

    // Obtiene evaluaciones filtradas por estado
    public async Task<IEnumerable<Evaluacion>> ObtenerPorEstadoAsync(EstadoEvaluacion estado)
    {
        return await _conjunto
            .Include(e => e.Joven)
            .Include(e => e.Curso)
            .Where(e => e.Estado == estado && e.Activo)
            .ToListAsync();
    }

    // Sobreescribe ObtenerPorIdAsync para incluir Joven y Curso por defecto
    public override async Task<Evaluacion?> ObtenerPorIdAsync(int id)
    {
        return await _conjunto
            .Include(e => e.Joven)
            .Include(e => e.Curso)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }
}
