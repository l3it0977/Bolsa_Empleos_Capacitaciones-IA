using System.Linq.Expressions;
using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Repositories;

// Implementacion generica del repositorio base.
// Proporciona las operaciones CRUD comunes para todas las entidades del dominio
// utilizando Entity Framework Core y PostgreSQL como base de datos.
public class Repositorio<TEntidad> : IRepositorio<TEntidad> where TEntidad : EntidadBase
{
    protected readonly BolsaEmpleosDbContext _contexto;
    protected readonly DbSet<TEntidad> _conjunto;

    public Repositorio(BolsaEmpleosDbContext contexto)
    {
        _contexto = contexto;
        _conjunto = contexto.Set<TEntidad>();
    }

    // Obtiene una entidad activa por su identificador unico
    public virtual async Task<TEntidad?> ObtenerPorIdAsync(int id)
    {
        return await _conjunto.FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }

    // Obtiene todas las entidades activas
    public virtual async Task<IEnumerable<TEntidad>> ObtenerTodosAsync()
    {
        return await _conjunto.Where(e => e.Activo).ToListAsync();
    }

    // Obtiene entidades activas que cumplen con el criterio especificado
    public virtual async Task<IEnumerable<TEntidad>> BuscarAsync(
        Expression<Func<TEntidad, bool>> criterio)
    {
        return await _conjunto.Where(criterio).ToListAsync();
    }

    // Agrega una nueva entidad y persiste los cambios
    public virtual async Task<TEntidad> AgregarAsync(TEntidad entidad)
    {
        await _conjunto.AddAsync(entidad);
        await _contexto.SaveChangesAsync();
        return entidad;
    }

    // Actualiza una entidad existente y persiste los cambios
    public virtual async Task ActualizarAsync(TEntidad entidad)
    {
        _conjunto.Update(entidad);
        await _contexto.SaveChangesAsync();
    }

    // Elimina logicamente una entidad marcando Activo = false
    public virtual async Task EliminarAsync(int id)
    {
        var entidad = await _conjunto.FindAsync(id);
        if (entidad is not null)
        {
            entidad.Activo = false;
            entidad.FechaModificacion = DateTime.UtcNow;
            await _contexto.SaveChangesAsync();
        }
    }

    // Verifica si existe alguna entidad que cumpla con el criterio especificado
    public virtual async Task<bool> ExisteAsync(Expression<Func<TEntidad, bool>> criterio)
    {
        return await _conjunto.AnyAsync(criterio);
    }
}
