using BolsaEmpleos.Application.DTOs.Empresa;

namespace BolsaEmpleos.Application.Interfaces;

// Define las operaciones del servicio de empresas de la plataforma.
// Esta interfaz desacopla la logica de negocio de su implementacion concreta.
public interface IServicioEmpresa
{
    // Obtiene todas las empresas activas registradas en la plataforma
    Task<IEnumerable<EmpresaDto>> ObtenerTodosAsync();

    // Obtiene una empresa por su identificador unico
    Task<EmpresaDto?> ObtenerPorIdAsync(int id);

    // Registra una nueva empresa en la plataforma
    Task<EmpresaDto> CrearAsync(CrearEmpresaDto dto);

    // Actualiza los datos de una empresa existente
    Task<EmpresaDto?> ActualizarAsync(int id, ActualizarEmpresaDto dto);

    // Elimina logicamente una empresa de la plataforma
    Task<bool> EliminarAsync(int id);
}
