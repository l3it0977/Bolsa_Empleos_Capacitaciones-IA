using AutoMapper;
using BolsaEmpleos.Application.DTOs.Curso;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de cursos.
// Los cursos cubren brechas de habilidades y al aprobarse enriquecen el CV.
public class ServicioCurso : IServicioCurso
{
    private readonly IRepositorioCurso _repositorioCurso;
    private readonly IRepositorioHabilidad _repositorioHabilidad;
    private readonly IMapper _mapper;

    public ServicioCurso(
        IRepositorioCurso repositorioCurso,
        IRepositorioHabilidad repositorioHabilidad,
        IMapper mapper)
    {
        _repositorioCurso = repositorioCurso;
        _repositorioHabilidad = repositorioHabilidad;
        _mapper = mapper;
    }

    // Obtiene todos los cursos activos disponibles en la plataforma
    public async Task<IEnumerable<CursoDto>> ObtenerTodosAsync()
    {
        var cursos = await _repositorioCurso.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<CursoDto>>(cursos);
    }

    // Obtiene un curso por su identificador unico
    public async Task<CursoDto?> ObtenerPorIdAsync(int id)
    {
        var curso = await _repositorioCurso.ObtenerPorIdAsync(id);
        return curso is null ? null : _mapper.Map<CursoDto>(curso);
    }

    // Obtiene los cursos que ensenan una habilidad especifica
    public async Task<IEnumerable<CursoDto>> ObtenerPorHabilidadAsync(int habilidadId)
    {
        var cursos = await _repositorioCurso.ObtenerPorHabilidadAsync(habilidadId);
        return _mapper.Map<IEnumerable<CursoDto>>(cursos);
    }

    // Crea un nuevo curso verificando que la habilidad asociada exista
    public async Task<CursoDto> CrearAsync(GuardarCursoDto dto)
    {
        // Verificar que la habilidad asociada existe
        var habilidad = await _repositorioHabilidad.ObtenerPorIdAsync(dto.HabilidadId);
        if (habilidad is null || !habilidad.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro una habilidad activa con el identificador {dto.HabilidadId}.");
        }

        var curso = _mapper.Map<Curso>(dto);
        var cursoCreado = await _repositorioCurso.AgregarAsync(curso);
        return _mapper.Map<CursoDto>(cursoCreado);
    }

    // Actualiza los datos de un curso existente
    public async Task<CursoDto?> ActualizarAsync(int id, GuardarCursoDto dto)
    {
        var curso = await _repositorioCurso.ObtenerPorIdAsync(id);
        if (curso is null) return null;

        curso.Titulo = dto.Titulo;
        curso.Descripcion = dto.Descripcion;
        curso.HabilidadId = dto.HabilidadId;
        curso.DuracionHoras = dto.DuracionHoras;
        curso.UrlMaterial = dto.UrlMaterial;
        curso.PuntajeMinimAprobacion = dto.PuntajeMinimAprobacion;
        curso.FechaModificacion = DateTime.UtcNow;

        await _repositorioCurso.ActualizarAsync(curso);
        return _mapper.Map<CursoDto>(curso);
    }

    // Elimina logicamente un curso del sistema
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _repositorioCurso.ExisteAsync(c => c.Id == id && c.Activo);
        if (!existe) return false;

        await _repositorioCurso.EliminarAsync(id);
        return true;
    }
}
