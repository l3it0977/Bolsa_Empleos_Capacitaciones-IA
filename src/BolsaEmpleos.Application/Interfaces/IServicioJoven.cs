using BolsaEmpleos.Application.DTOs.Joven;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de jovenes de la plataforma.
// Esta interfaz desacopla la logica de negocio de su implementacion concreta.
public interface IServicioJoven
{
    // Obtiene todos los jovenes activos registrados en la plataforma
    Task<IEnumerable<JovenDto>> ObtenerTodosAsync();

    // Obtiene un joven por su identificador unico
    Task<JovenDto?> ObtenerPorIdAsync(int id);

    // Registra un nuevo joven en la plataforma
    Task<JovenDto> CrearAsync(CrearJovenDto dto);

    // Actualiza los datos de un joven existente
    Task<JovenDto?> ActualizarAsync(int id, ActualizarJovenDto dto);

    // Elimina logicamente un joven de la plataforma
    Task<bool> EliminarAsync(int id);
}
