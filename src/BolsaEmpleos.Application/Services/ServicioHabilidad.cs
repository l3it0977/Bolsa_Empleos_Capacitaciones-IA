using AutoMapper;
using BolsaEmpleos.Application.DTOs.Habilidad;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de habilidades.
// Las habilidades son el nucleo del sistema de brechas y capacitacion.
public class ServicioHabilidad : IServicioHabilidad
{
    private readonly IRepositorioHabilidad _repositorioHabilidad;
    private readonly IMapper _mapper;

    public ServicioHabilidad(IRepositorioHabilidad repositorioHabilidad, IMapper mapper)
    {
        _repositorioHabilidad = repositorioHabilidad;
        _mapper = mapper;
    }

    // Obtiene todas las habilidades activas del sistema
    public async Task<IEnumerable<HabilidadDto>> ObtenerTodosAsync()
    {
        var habilidades = await _repositorioHabilidad.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<HabilidadDto>>(habilidades);
    }

    // Obtiene una habilidad por su identificador unico
    public async Task<HabilidadDto?> ObtenerPorIdAsync(int id)
    {
        var habilidad = await _repositorioHabilidad.ObtenerPorIdAsync(id);
        return habilidad is null ? null : _mapper.Map<HabilidadDto>(habilidad);
    }

    // Busca habilidades cuyo nombre contenga el texto proporcionado
    public async Task<IEnumerable<HabilidadDto>> BuscarPorNombreAsync(string nombre)
    {
        var habilidades = await _repositorioHabilidad.BuscarPorNombreAsync(nombre);
        return _mapper.Map<IEnumerable<HabilidadDto>>(habilidades);
    }

    // Obtiene habilidades de una categoria especifica
    public async Task<IEnumerable<HabilidadDto>> ObtenerPorCategoriaAsync(string categoria)
    {
        var habilidades = await _repositorioHabilidad.ObtenerPorCategoriaAsync(categoria);
        return _mapper.Map<IEnumerable<HabilidadDto>>(habilidades);
    }

    // Crea una nueva habilidad verificando que el nombre sea unico
    public async Task<HabilidadDto> CrearAsync(GuardarHabilidadDto dto)
    {
        // Verificar que no exista otra habilidad con el mismo nombre
        var existente = await _repositorioHabilidad.ExisteAsync(
            h => h.Nombre.ToLower() == dto.Nombre.ToLower() && h.Activo);

        if (existente)
        {
            throw new InvalidOperationException(
                $"Ya existe una habilidad con el nombre '{dto.Nombre}'.");
        }

        var habilidad = _mapper.Map<Habilidad>(dto);
        var habilidadCreada = await _repositorioHabilidad.AgregarAsync(habilidad);
        return _mapper.Map<HabilidadDto>(habilidadCreada);
    }

    // Actualiza los datos de una habilidad existente
    public async Task<HabilidadDto?> ActualizarAsync(int id, GuardarHabilidadDto dto)
    {
        var habilidad = await _repositorioHabilidad.ObtenerPorIdAsync(id);
        if (habilidad is null) return null;

        habilidad.Nombre = dto.Nombre;
        habilidad.Descripcion = dto.Descripcion;
        habilidad.Categoria = dto.Categoria;
        habilidad.FechaModificacion = DateTime.UtcNow;

        await _repositorioHabilidad.ActualizarAsync(habilidad);
        return _mapper.Map<HabilidadDto>(habilidad);
    }

    // Elimina logicamente una habilidad del sistema
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _repositorioHabilidad.ExisteAsync(h => h.Id == id && h.Activo);
        if (!existe) return false;

        await _repositorioHabilidad.EliminarAsync(id);
        return true;
    }
}
