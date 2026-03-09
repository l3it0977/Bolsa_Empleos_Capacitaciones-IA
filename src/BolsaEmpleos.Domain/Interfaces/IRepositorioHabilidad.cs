using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Habilidad.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioHabilidad : IRepositorio<Habilidad>
{
    // Busca habilidades cuyo nombre contenga el texto especificado
    Task<IEnumerable<Habilidad>> BuscarPorNombreAsync(string nombre);

    // Obtiene habilidades pertenecientes a una categoria especifica
    Task<IEnumerable<Habilidad>> ObtenerPorCategoriaAsync(string categoria);
}
