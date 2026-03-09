using BolsaEmpleos.Application.DTOs.Habilidad;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de habilidades del sistema.
// Las habilidades son el nodo central del modelo de brechas de capacitacion.
public interface IServicioHabilidad
{
    // Obtiene todas las habilidades activas en el sistema
    Task<IEnumerable<HabilidadDto>> ObtenerTodosAsync();

    // Obtiene una habilidad por su identificador unico
    Task<HabilidadDto?> ObtenerPorIdAsync(int id);

    // Busca habilidades por nombre (busqueda parcial)
    Task<IEnumerable<HabilidadDto>> BuscarPorNombreAsync(string nombre);

    // Obtiene habilidades de una categoria especifica
    Task<IEnumerable<HabilidadDto>> ObtenerPorCategoriaAsync(string categoria);

    // Crea una nueva habilidad en el sistema
    Task<HabilidadDto> CrearAsync(GuardarHabilidadDto dto);

    // Actualiza una habilidad existente
    Task<HabilidadDto?> ActualizarAsync(int id, GuardarHabilidadDto dto);

    // Elimina logicamente una habilidad del sistema
    Task<bool> EliminarAsync(int id);
}
