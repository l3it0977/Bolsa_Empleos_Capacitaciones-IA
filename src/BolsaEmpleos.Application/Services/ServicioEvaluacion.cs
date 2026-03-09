using AutoMapper;
using BolsaEmpleos.Application.DTOs.Evaluacion;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de evaluaciones.
// Coordina el flujo: iniciar evaluacion -> registrar resultado -> actualizar curriculum.
// Este servicio es el nucleo del mecanismo de capacitacion de la plataforma.
public class ServicioEvaluacion : IServicioEvaluacion
{
    private readonly IRepositorioEvaluacion _repositorioEvaluacion;
    private readonly IRepositorioCurso _repositorioCurso;
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IServicioCurriculum _servicioCurriculum;
    private readonly IMapper _mapper;

    public ServicioEvaluacion(
        IRepositorioEvaluacion repositorioEvaluacion,
        IRepositorioCurso repositorioCurso,
        IRepositorioJoven repositorioJoven,
        IServicioCurriculum servicioCurriculum,
        IMapper mapper)
    {
        _repositorioEvaluacion = repositorioEvaluacion;
        _repositorioCurso = repositorioCurso;
        _repositorioJoven = repositorioJoven;
        _servicioCurriculum = servicioCurriculum;
        _mapper = mapper;
    }

    // Obtiene todas las evaluaciones de un joven especifico
    public async Task<IEnumerable<EvaluacionDto>> ObtenerPorJovenAsync(int jovenId)
    {
        var evaluaciones = await _repositorioEvaluacion.ObtenerPorJovenAsync(jovenId);
        return _mapper.Map<IEnumerable<EvaluacionDto>>(evaluaciones);
    }

    // Obtiene una evaluacion por su identificador unico
    public async Task<EvaluacionDto?> ObtenerPorIdAsync(int id)
    {
        var evaluacion = await _repositorioEvaluacion.ObtenerPorIdAsync(id);
        return evaluacion is null ? null : _mapper.Map<EvaluacionDto>(evaluacion);
    }

    // Inicia una nueva evaluacion de curso para un joven.
    // Verifica que el joven y el curso existan y que no tenga una evaluacion aprobada previa.
    public async Task<EvaluacionDto> IniciarEvaluacionAsync(int jovenId, int cursoId)
    {
        // Verificar que el joven existe
        var joven = await _repositorioJoven.ObtenerPorIdAsync(jovenId);
        if (joven is null || !joven.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un joven activo con el identificador {jovenId}.");
        }

        // Verificar que el curso existe
        var curso = await _repositorioCurso.ObtenerPorIdAsync(cursoId);
        if (curso is null || !curso.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un curso activo con el identificador {cursoId}.");
        }

        // Verificar que no tenga una evaluacion aprobada para este curso
        var evaluacionAprobada = await _repositorioEvaluacion.ObtenerPorJovenYCursoAsync(jovenId, cursoId);
        if (evaluacionAprobada is not null && evaluacionAprobada.Estado == EstadoEvaluacion.Aprobada)
        {
            throw new InvalidOperationException(
                $"El joven ya aprobo el curso '{curso.Titulo}' anteriormente.");
        }

        // Crear la nueva evaluacion en estado pendiente
        var evaluacion = new Evaluacion
        {
            JovenId = jovenId,
            CursoId = cursoId,
            Estado = EstadoEvaluacion.EnCurso,
            FechaInicio = DateTime.UtcNow,
            Intentos = (evaluacionAprobada?.Intentos ?? 0) + 1
        };

        var evaluacionCreada = await _repositorioEvaluacion.AgregarAsync(evaluacion);
        return _mapper.Map<EvaluacionDto>(evaluacionCreada);
    }

    // Registra el resultado de una evaluacion.
    // Si el puntaje supera el minimo del curso, se aprueba y se agrega la habilidad al curriculum.
    public async Task<EvaluacionDto?> RegistrarResultadoAsync(
        int evaluacionId, RegistrarResultadoEvaluacionDto dto)
    {
        var evaluacion = await _repositorioEvaluacion.ObtenerPorIdAsync(evaluacionId);
        if (evaluacion is null) return null;

        // Obtener el curso para conocer el puntaje minimo de aprobacion
        var curso = await _repositorioCurso.ObtenerPorIdAsync(evaluacion.CursoId);
        if (curso is null) return null;

        evaluacion.PuntajeObtenido = dto.PuntajeObtenido;
        evaluacion.FechaFin = DateTime.UtcNow;
        evaluacion.FechaModificacion = DateTime.UtcNow;

        // Determinar si el joven aprobo segun el puntaje minimo definido en el curso
        if (dto.PuntajeObtenido >= curso.PuntajeMinimAprobacion)
        {
            evaluacion.Estado = EstadoEvaluacion.Aprobada;

            // Agregar la habilidad al curriculum del joven como adquirida por curso
            await _servicioCurriculum.AgregarHabilidadAsync(
                evaluacion.JovenId, curso.HabilidadId, obtenidaPorCurso: true);
        }
        else
        {
            evaluacion.Estado = EstadoEvaluacion.Reprobada;
        }

        await _repositorioEvaluacion.ActualizarAsync(evaluacion);
        return _mapper.Map<EvaluacionDto>(evaluacion);
    }
}
