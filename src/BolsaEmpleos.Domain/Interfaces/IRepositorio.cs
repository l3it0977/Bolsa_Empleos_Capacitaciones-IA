using System.Linq.Expressions;
using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Interfaces;

// Interfaz generica de repositorio que define las operaciones CRUD basicas
// para cualquier entidad del dominio. Sigue el patron Repository para desacoplar
// la capa de dominio de la implementacion de acceso a datos.
public interface IRepositorio<TEntidad> where TEntidad : EntidadBase
{
    // Obtiene una entidad por su identificador unico
    Task<TEntidad?> ObtenerPorIdAsync(int id);

    // Obtiene todas las entidades activas del tipo especificado
    Task<IEnumerable<TEntidad>> ObtenerTodosAsync();

    // Obtiene entidades que cumplan con el criterio especificado
    Task<IEnumerable<TEntidad>> BuscarAsync(Expression<Func<TEntidad, bool>> criterio);

    // Agrega una nueva entidad al repositorio
    Task<TEntidad> AgregarAsync(TEntidad entidad);

    // Actualiza una entidad existente en el repositorio
    Task ActualizarAsync(TEntidad entidad);

    // Elimina logicamente una entidad (cambia Activo a false)
    Task EliminarAsync(int id);

    // Verifica si existe alguna entidad que cumpla con el criterio especificado
    Task<bool> ExisteAsync(Expression<Func<TEntidad, bool>> criterio);
}
