using AutoMapper;
using BolsaEmpleos.Application.DTOs.Curriculum;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de curricula.
// Permite crear, actualizar y enriquecer el CV del joven con habilidades
// adquiridas mediante la aprobacion de cursos de capacitacion.
public class ServicioCurriculum : IServicioCurriculum
{
    private readonly IRepositorioCurriculum _repositorioCurriculum;
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IMapper _mapper;

    public ServicioCurriculum(
        IRepositorioCurriculum repositorioCurriculum,
        IRepositorioJoven repositorioJoven,
        IMapper mapper)
    {
        _repositorioCurriculum = repositorioCurriculum;
        _repositorioJoven = repositorioJoven;
        _mapper = mapper;
    }

    // Obtiene el curriculum de un joven con todas sus habilidades
    public async Task<CurriculumDto?> ObtenerPorJovenAsync(int jovenId)
    {
        var curriculum = await _repositorioCurriculum.ObtenerPorJovenAsync(jovenId);
        return curriculum is null ? null : _mapper.Map<CurriculumDto>(curriculum);
    }

    // Crea un nuevo curriculum o actualiza el existente del joven.
    // Los datos personales se heredan automaticamente desde la entidad Joven.
    public async Task<CurriculumDto> GuardarAsync(int jovenId, GuardarCurriculumDto dto)
    {
        // Verificar que el joven existe
        var joven = await _repositorioJoven.ObtenerPorIdAsync(jovenId);
        if (joven is null || !joven.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un joven activo con el identificador {jovenId}.");
        }

        // Verificar si ya existe un curriculum para este joven
        var curriculumExistente = await _repositorioCurriculum.ObtenerPorJovenAsync(jovenId);

        Curriculum curriculum;
        if (curriculumExistente is not null)
        {
            // Actualizar el curriculum existente
            curriculumExistente.ResumenProfesional = dto.ResumenProfesional;
            curriculumExistente.TituloProfesional = dto.TituloProfesional;
            curriculumExistente.UrlPortfolio = dto.UrlPortfolio;
            curriculumExistente.FechaModificacion = DateTime.UtcNow;
            await _repositorioCurriculum.ActualizarAsync(curriculumExistente);
            curriculum = curriculumExistente;
        }
        else
        {
            // Crear un nuevo curriculum asociado al joven
            var nuevoCurriculum = _mapper.Map<Curriculum>(dto);
            nuevoCurriculum.JovenId = jovenId;
            curriculum = await _repositorioCurriculum.AgregarAsync(nuevoCurriculum);
        }

        // Recargar con datos completos del joven para el mapeo
        var curriculumCompleto = await _repositorioCurriculum.ObtenerPorJovenAsync(jovenId);
        return _mapper.Map<CurriculumDto>(curriculumCompleto!);
    }

    // Agrega una habilidad al curriculum del joven, indicando si fue obtenida por curso
    public async Task<bool> AgregarHabilidadAsync(int jovenId, int habilidadId, bool obtenidaPorCurso)
    {
        var curriculum = await _repositorioCurriculum.ObtenerPorJovenAsync(jovenId);
        if (curriculum is null) return false;

        // Verificar si la habilidad ya esta en el curriculum
        var yaExiste = await _repositorioCurriculum.TieneHabilidadAsync(curriculum.Id, habilidadId);
        if (yaExiste) return false;

        await _repositorioCurriculum.AgregarHabilidadAsync(curriculum.Id, habilidadId, obtenidaPorCurso);
        return true;
    }
}
