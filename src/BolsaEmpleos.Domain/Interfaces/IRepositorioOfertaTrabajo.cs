using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad OfertaTrabajo.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioOfertaTrabajo : IRepositorio<OfertaTrabajo>
{
    // Obtiene las ofertas de trabajo de una empresa especifica
    Task<IEnumerable<OfertaTrabajo>> ObtenerPorEmpresaAsync(int empresaId);

    // Obtiene las ofertas de trabajo por su estado actual
    Task<IEnumerable<OfertaTrabajo>> ObtenerPorEstadoAsync(EstadoOferta estado);

    // Obtiene una oferta de trabajo junto con todos sus requisitos y habilidades
    Task<OfertaTrabajo?> ObtenerConRequisitosAsync(int ofertaId);

    // Obtiene ofertas que contengan al menos una habilidad especificada
    Task<IEnumerable<OfertaTrabajo>> BuscarPorHabilidadAsync(int habilidadId);
}
