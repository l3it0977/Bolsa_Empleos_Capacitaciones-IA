using AutoMapper;
using BolsaEmpleos.Application.DTOs.Joven;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de jovenes.
// Coordina las operaciones CRUD y aplica reglas de negocio como la encriptacion
// de la contrasena antes de persistir el registro.
public class ServicioJoven : IServicioJoven
{
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IMapper _mapper;

    public ServicioJoven(IRepositorioJoven repositorioJoven, IMapper mapper)
    {
        _repositorioJoven = repositorioJoven;
        _mapper = mapper;
    }

    // Obtiene todos los jovenes activos registrados en la plataforma
    public async Task<IEnumerable<JovenDto>> ObtenerTodosAsync()
    {
        var jovenes = await _repositorioJoven.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<JovenDto>>(jovenes);
    }

    // Obtiene un joven especifico por su identificador unico
    public async Task<JovenDto?> ObtenerPorIdAsync(int id)
    {
        var joven = await _repositorioJoven.ObtenerPorIdAsync(id);
        return joven is null ? null : _mapper.Map<JovenDto>(joven);
    }

    // Registra un nuevo joven en la plataforma con la contrasena encriptada
    public async Task<JovenDto> CrearAsync(CrearJovenDto dto)
    {
        // Verificar que no exista otro joven con el mismo correo electronico
        var existente = await _repositorioJoven.ObtenerPorCorreoAsync(dto.CorreoElectronico);
        if (existente is not null)
        {
            throw new InvalidOperationException(
                $"Ya existe un joven registrado con el correo '{dto.CorreoElectronico}'.");
        }

        // Mapear DTO a entidad y encriptar la contrasena
        var joven = _mapper.Map<Joven>(dto);
        joven.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        var jovenCreado = await _repositorioJoven.AgregarAsync(joven);
        return _mapper.Map<JovenDto>(jovenCreado);
    }

    // Actualiza los datos editables de un joven existente
    public async Task<JovenDto?> ActualizarAsync(int id, ActualizarJovenDto dto)
    {
        var joven = await _repositorioJoven.ObtenerPorIdAsync(id);
        if (joven is null) return null;

        joven.Nombre = dto.Nombre;
        joven.Apellido = dto.Apellido;
        joven.Telefono = dto.Telefono;
        joven.NivelEducativo = dto.NivelEducativo;
        joven.FechaModificacion = DateTime.UtcNow;

        await _repositorioJoven.ActualizarAsync(joven);
        return _mapper.Map<JovenDto>(joven);
    }

    // Elimina logicamente un joven de la plataforma
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _repositorioJoven.ExisteAsync(j => j.Id == id && j.Activo);
        if (!existe) return false;

        await _repositorioJoven.EliminarAsync(id);
        return true;
    }
}
