using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Domain.Interfaces;

// Repositorio especializado para la entidad Empresa.
// Extiende las operaciones CRUD basicas con consultas especificas del negocio.
public interface IRepositorioEmpresa : IRepositorio<Empresa>
{
    // Busca una empresa por su correo electronico (usado en autenticacion)
    Task<Empresa?> ObtenerPorCorreoAsync(string correoElectronico);

    // Obtiene una empresa junto con todas sus ofertas de trabajo publicadas
    Task<Empresa?> ObtenerConOfertasAsync(int empresaId);
}
