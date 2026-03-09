using BolsaEmpleos.Application.DTOs.OfertaTrabajo;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de ofertas de trabajo.
// Gestiona el ciclo de vida completo de las ofertas publicadas por empresas.
public interface IServicioOfertaTrabajo
{
    // Obtiene todas las ofertas de trabajo publicadas (estado Publicada)
    Task<IEnumerable<OfertaTrabajoDto>> ObtenerPublicadasAsync();

    // Obtiene todas las ofertas de trabajo de una empresa especifica
    Task<IEnumerable<OfertaTrabajoDto>> ObtenerPorEmpresaAsync(int empresaId);

    // Obtiene una oferta de trabajo por su identificador unico
    Task<OfertaTrabajoDto?> ObtenerPorIdAsync(int id);

    // Crea una nueva oferta de trabajo para una empresa
    Task<OfertaTrabajoDto> CrearAsync(int empresaId, CrearOfertaTrabajoDto dto);

    // Actualiza el estado de una oferta de trabajo
    Task<bool> CambiarEstadoAsync(int ofertaId, EstadoOferta nuevoEstado);

    // Elimina logicamente una oferta de trabajo
    Task<bool> EliminarAsync(int id);
}
