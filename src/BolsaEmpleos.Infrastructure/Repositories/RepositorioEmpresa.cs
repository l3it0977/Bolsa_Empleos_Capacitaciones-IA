using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de empresas.
// Extiende el repositorio generico con consultas especificas del negocio.
public class RepositorioEmpresa : Repositorio<Empresa>, IRepositorioEmpresa
{
    public RepositorioEmpresa(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Busca una empresa activa por su correo electronico
    public async Task<Empresa?> ObtenerPorCorreoAsync(string correoElectronico)
    {
        return await _conjunto
            .FirstOrDefaultAsync(e =>
                e.CorreoElectronico == correoElectronico && e.Activo);
    }

    // Obtiene una empresa con todas sus ofertas de trabajo activas
    public async Task<Empresa?> ObtenerConOfertasAsync(int empresaId)
    {
        return await _conjunto
            .Include(e => e.OfertasTrabajo.Where(o => o.Activo))
                .ThenInclude(o => o.Requisitos.Where(r => r.Activo))
                    .ThenInclude(r => r.Habilidad)
            .FirstOrDefaultAsync(e => e.Id == empresaId && e.Activo);
    }
}
