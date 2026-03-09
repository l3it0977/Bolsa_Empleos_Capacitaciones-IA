using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de ofertas de trabajo.
// Extiende el repositorio generico con consultas especificas del negocio.
public class RepositorioOfertaTrabajo : Repositorio<OfertaTrabajo>, IRepositorioOfertaTrabajo
{
    public RepositorioOfertaTrabajo(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Obtiene todas las ofertas activas de una empresa especifica
    public async Task<IEnumerable<OfertaTrabajo>> ObtenerPorEmpresaAsync(int empresaId)
    {
        return await _conjunto
            .Include(o => o.Empresa)
            .Include(o => o.Requisitos.Where(r => r.Activo))
                .ThenInclude(r => r.Habilidad)
            .Where(o => o.EmpresaId == empresaId && o.Activo)
            .ToListAsync();
    }

    // Obtiene todas las ofertas activas con un estado especifico
    public async Task<IEnumerable<OfertaTrabajo>> ObtenerPorEstadoAsync(EstadoOferta estado)
    {
        return await _conjunto
            .Include(o => o.Empresa)
            .Include(o => o.Requisitos.Where(r => r.Activo))
                .ThenInclude(r => r.Habilidad)
            .Where(o => o.Estado == estado && o.Activo)
            .ToListAsync();
    }

    // Obtiene una oferta con todos sus requisitos y habilidades asociadas
    public async Task<OfertaTrabajo?> ObtenerConRequisitosAsync(int ofertaId)
    {
        return await _conjunto
            .Include(o => o.Empresa)
            .Include(o => o.Requisitos.Where(r => r.Activo))
                .ThenInclude(r => r.Habilidad)
            .FirstOrDefaultAsync(o => o.Id == ofertaId && o.Activo);
    }

    // Obtiene ofertas que contienen un requisito para la habilidad especificada
    public async Task<IEnumerable<OfertaTrabajo>> BuscarPorHabilidadAsync(int habilidadId)
    {
        return await _conjunto
            .Include(o => o.Empresa)
            .Include(o => o.Requisitos.Where(r => r.Activo))
                .ThenInclude(r => r.Habilidad)
            .Where(o => o.Activo && o.Requisitos.Any(r => r.HabilidadId == habilidadId && r.Activo))
            .ToListAsync();
    }
}
