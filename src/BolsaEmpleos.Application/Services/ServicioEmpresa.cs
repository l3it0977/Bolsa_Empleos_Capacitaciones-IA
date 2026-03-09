using AutoMapper;
using BolsaEmpleos.Application.DTOs.Empresa;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de empresas.
// Coordina las operaciones CRUD y aplica reglas de negocio como la encriptacion
// de la contrasena antes de persistir el registro.
public class ServicioEmpresa : IServicioEmpresa
{
    private readonly IRepositorioEmpresa _repositorioEmpresa;
    private readonly IMapper _mapper;

    public ServicioEmpresa(IRepositorioEmpresa repositorioEmpresa, IMapper mapper)
    {
        _repositorioEmpresa = repositorioEmpresa;
        _mapper = mapper;
    }

    // Obtiene todas las empresas activas registradas en la plataforma
    public async Task<IEnumerable<EmpresaDto>> ObtenerTodosAsync()
    {
        var empresas = await _repositorioEmpresa.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
    }

    // Obtiene una empresa especifica por su identificador unico
    public async Task<EmpresaDto?> ObtenerPorIdAsync(int id)
    {
        var empresa = await _repositorioEmpresa.ObtenerPorIdAsync(id);
        return empresa is null ? null : _mapper.Map<EmpresaDto>(empresa);
    }

    // Registra una nueva empresa en la plataforma con la contrasena encriptada
    public async Task<EmpresaDto> CrearAsync(CrearEmpresaDto dto)
    {
        // Verificar que no exista otra empresa con el mismo correo electronico
        var existente = await _repositorioEmpresa.ObtenerPorCorreoAsync(dto.CorreoElectronico);
        if (existente is not null)
        {
            throw new InvalidOperationException(
                $"Ya existe una empresa registrada con el correo '{dto.CorreoElectronico}'.");
        }

        // Mapear DTO a entidad y encriptar la contrasena
        var empresa = _mapper.Map<Empresa>(dto);
        empresa.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        var empresaCreada = await _repositorioEmpresa.AgregarAsync(empresa);
        return _mapper.Map<EmpresaDto>(empresaCreada);
    }

    // Actualiza los datos editables de una empresa existente
    public async Task<EmpresaDto?> ActualizarAsync(int id, ActualizarEmpresaDto dto)
    {
        var empresa = await _repositorioEmpresa.ObtenerPorIdAsync(id);
        if (empresa is null) return null;

        empresa.RazonSocial = dto.RazonSocial;
        empresa.Telefono = dto.Telefono;
        empresa.Descripcion = dto.Descripcion;
        empresa.SitioWeb = dto.SitioWeb;
        empresa.FechaModificacion = DateTime.UtcNow;

        await _repositorioEmpresa.ActualizarAsync(empresa);
        return _mapper.Map<EmpresaDto>(empresa);
    }

    // Elimina logicamente una empresa de la plataforma
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _repositorioEmpresa.ExisteAsync(e => e.Id == id && e.Activo);
        if (!existe) return false;

        await _repositorioEmpresa.EliminarAsync(id);
        return true;
    }
}
