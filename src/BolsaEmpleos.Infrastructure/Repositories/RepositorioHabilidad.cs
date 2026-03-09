using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion del repositorio de habilidades.
public class RepositorioHabilidad : Repositorio<Habilidad>, IRepositorioHabilidad
{
    public RepositorioHabilidad(BolsaEmpleosDbContext contexto) : base(contexto)
    {
    }

    // Busca habilidades activas cuyo nombre contenga el texto especificado
    public async Task<IEnumerable<Habilidad>> BuscarPorNombreAsync(string nombre)
    {
        return await _conjunto
            .Where(h => h.Activo && h.Nombre.ToLower().Contains(nombre.ToLower()))
            .ToListAsync();
    }

    // Obtiene habilidades activas de una categoria especifica
    public async Task<IEnumerable<Habilidad>> ObtenerPorCategoriaAsync(string categoria)
    {
        return await _conjunto
            .Where(h => h.Activo && h.Categoria == categoria)
            .ToListAsync();
    }
}
