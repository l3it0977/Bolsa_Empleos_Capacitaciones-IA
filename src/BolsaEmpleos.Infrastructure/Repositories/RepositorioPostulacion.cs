using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de postulaciones.
// Extiende el repositorio generico con consultas especificas del negocio.
public class RepositorioPostulacion : Repositorio<Postulacion>, IRepositorioPostulacion
{
    public RepositorioPostulacion(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Obtiene todas las postulaciones activas de un joven especifico
    public async Task<IEnumerable<Postulacion>> ObtenerPorJovenAsync(int jovenId)
    {
        return await _conjunto
            .Include(p => p.Joven)
            .Include(p => p.OfertaTrabajo)
            .Where(p => p.JovenId == jovenId && p.Activo)
            .OrderByDescending(p => p.FechaPostulacion)
            .ToListAsync();
    }

    // Obtiene todas las postulaciones activas de una oferta de trabajo especifica
    public async Task<IEnumerable<Postulacion>> ObtenerPorOfertaAsync(int ofertaTrabajoId)
    {
        return await _conjunto
            .Include(p => p.Joven)
            .Include(p => p.OfertaTrabajo)
            .Where(p => p.OfertaTrabajoId == ofertaTrabajoId && p.Activo)
            .OrderByDescending(p => p.FechaPostulacion)
            .ToListAsync();
    }

    // Verifica si el joven ya tiene una postulacion activa para la oferta indicada
    public async Task<Postulacion?> ObtenerPorJovenYOfertaAsync(int jovenId, int ofertaTrabajoId)
    {
        return await _conjunto
            .Include(p => p.Joven)
            .Include(p => p.OfertaTrabajo)
            .FirstOrDefaultAsync(p =>
                p.JovenId == jovenId &&
                p.OfertaTrabajoId == ofertaTrabajoId &&
                p.Activo);
    }

    // Obtiene postulaciones filtradas por estado
    public async Task<IEnumerable<Postulacion>> ObtenerPorEstadoAsync(EstadoPostulacion estado)
    {
        return await _conjunto
            .Include(p => p.Joven)
            .Include(p => p.OfertaTrabajo)
            .Where(p => p.Estado == estado && p.Activo)
            .OrderByDescending(p => p.FechaPostulacion)
            .ToListAsync();
    }

    // Sobreescribe ObtenerPorIdAsync para incluir Joven y OfertaTrabajo por defecto
    public override async Task<Postulacion?> ObtenerPorIdAsync(int id)
    {
        return await _conjunto
            .Include(p => p.Joven)
            .Include(p => p.OfertaTrabajo)
            .FirstOrDefaultAsync(p => p.Id == id && p.Activo);
    }
}
