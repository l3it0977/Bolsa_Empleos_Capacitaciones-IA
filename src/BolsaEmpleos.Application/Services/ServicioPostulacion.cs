using AutoMapper;
using BolsaEmpleos.Application.DTOs.Postulacion;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio de aplicacion encargado de la logica de postulacion a ofertas de trabajo.
// Compara el CV del joven con los requisitos del puesto, determina si puede postular
// directamente, y lista los cursos necesarios cuando faltan habilidades no relevantes.
public class ServicioPostulacion : IServicioPostulacion
{
    private readonly IRepositorioPostulacion _repositorioPostulacion;
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IRepositorioOfertaTrabajo _repositorioOfertaTrabajo;
    private readonly IRepositorioCurso _repositorioCurso;
    private readonly IMapper _mapper;

    public ServicioPostulacion(
        IRepositorioPostulacion repositorioPostulacion,
        IRepositorioJoven repositorioJoven,
        IRepositorioOfertaTrabajo repositorioOfertaTrabajo,
        IRepositorioCurso repositorioCurso,
        IMapper mapper)
    {
        _repositorioPostulacion = repositorioPostulacion;
        _repositorioJoven = repositorioJoven;
        _repositorioOfertaTrabajo = repositorioOfertaTrabajo;
        _repositorioCurso = repositorioCurso;
        _mapper = mapper;
    }

    // Evalua si el joven puede postularse a la oferta comparando su curriculum con los requisitos.
    // Reglas:
    //   - Si falta un requisito Relevante -> no puede postular (bloqueo total).
    //   - Si faltan requisitos NoRelevante -> debe completar los cursos sugeridos antes de postular.
    //   - Si cumple todos los requisitos -> puede postular directamente.
    public async Task<ResultadoEvaluacionPostulacionDto> EvaluarPostulacionAsync(
        int jovenId, int ofertaTrabajoId)
    {
        // Obtener el joven con su curriculum y habilidades
        var joven = await _repositorioJoven.ObtenerConCurriculumAsync(jovenId);
        if (joven is null || !joven.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un joven activo con el identificador {jovenId}.");
        }

        // Obtener la oferta con todos sus requisitos y habilidades asociadas
        var oferta = await _repositorioOfertaTrabajo.ObtenerConRequisitosAsync(ofertaTrabajoId);
        if (oferta is null || !oferta.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro una oferta activa con el identificador {ofertaTrabajoId}.");
        }

        // Verificar que la oferta este publicada para poder postularse
        if (oferta.Estado != EstadoOferta.Publicada)
        {
            throw new InvalidOperationException(
                $"La oferta '{oferta.Titulo}' no esta disponible para postulaciones.");
        }

        // Obtener los identificadores de habilidades que el joven tiene en su curriculum
        var habilidadesJoven = ObtenerHabilidadesDelCurriculum(joven);

        // Separar los requisitos de la oferta por tipo
        var requisitosRelevantes = oferta.Requisitos
            .Where(r => r.TipoRequisito == TipoRequisito.Relevante)
            .ToList();

        var requisitosNoRelevantes = oferta.Requisitos
            .Where(r => r.TipoRequisito == TipoRequisito.NoRelevante)
            .ToList();

        // Identificar habilidades relevantes que el joven no posee
        var habilidadesRelevantesFaltantes = requisitosRelevantes
            .Where(r => !habilidadesJoven.Contains(r.HabilidadId))
            .Select(r => new HabilidadFaltanteDto
            {
                HabilidadId = r.HabilidadId,
                NombreHabilidad = r.Habilidad.Nombre,
                DescripcionRequisito = r.Descripcion
            })
            .ToList();

        // Si falta al menos una habilidad relevante, el joven no puede postularse
        if (habilidadesRelevantesFaltantes.Count > 0)
        {
            return new ResultadoEvaluacionPostulacionDto
            {
                JovenId = jovenId,
                OfertaTrabajoId = ofertaTrabajoId,
                TituloOferta = oferta.Titulo,
                PuedePostular = false,
                MotivoBloqueo =
                    "El joven no cumple con los requisitos obligatorios del puesto. " +
                    "Debe adquirir las habilidades relevantes faltantes para poder postularse.",
                HabilidadesRelevantesFaltantes = habilidadesRelevantesFaltantes,
                CursosSugeridos = new List<CursoSugeridoDto>()
            };
        }

        // Identificar requisitos no relevantes que el joven no posee
        var requisitosNoRelevantesFaltantes = requisitosNoRelevantes
            .Where(r => !habilidadesJoven.Contains(r.HabilidadId))
            .ToList();

        // Si faltan habilidades no relevantes, buscar cursos para cubrir cada brecha
        if (requisitosNoRelevantesFaltantes.Count > 0)
        {
            var cursosSugeridos = await ObtenerCursosSugeridosAsync(requisitosNoRelevantesFaltantes);

            return new ResultadoEvaluacionPostulacionDto
            {
                JovenId = jovenId,
                OfertaTrabajoId = ofertaTrabajoId,
                TituloOferta = oferta.Titulo,
                PuedePostular = false,
                MotivoBloqueo =
                    "El joven debe completar los cursos sugeridos para cubrir " +
                    "las habilidades complementarias requeridas antes de postularse.",
                HabilidadesRelevantesFaltantes = new List<HabilidadFaltanteDto>(),
                CursosSugeridos = cursosSugeridos
            };
        }

        // El joven cumple con todos los requisitos y puede postularse directamente
        return new ResultadoEvaluacionPostulacionDto
        {
            JovenId = jovenId,
            OfertaTrabajoId = ofertaTrabajoId,
            TituloOferta = oferta.Titulo,
            PuedePostular = true,
            MotivoBloqueo = null,
            HabilidadesRelevantesFaltantes = new List<HabilidadFaltanteDto>(),
            CursosSugeridos = new List<CursoSugeridoDto>()
        };
    }

    // Registra la postulacion del joven a la oferta si cumple con todos los requisitos.
    // Primero evalua la postulacion y solo la registra si puede postular directamente.
    public async Task<PostulacionDto> PostularAsync(int jovenId, int ofertaTrabajoId)
    {
        // Evaluar si el joven puede postularse
        var resultado = await EvaluarPostulacionAsync(jovenId, ofertaTrabajoId);

        if (!resultado.PuedePostular)
        {
            throw new InvalidOperationException(resultado.MotivoBloqueo);
        }

        // Verificar si el joven ya tiene una postulacion activa para esta oferta
        var postulacionExistente = await _repositorioPostulacion
            .ObtenerPorJovenYOfertaAsync(jovenId, ofertaTrabajoId);

        if (postulacionExistente is not null)
        {
            throw new InvalidOperationException(
                "El joven ya tiene una postulacion registrada para esta oferta de trabajo.");
        }

        // Registrar la nueva postulacion
        var postulacion = new Postulacion
        {
            JovenId = jovenId,
            OfertaTrabajoId = ofertaTrabajoId,
            Estado = EstadoPostulacion.Pendiente,
            FechaPostulacion = DateTime.UtcNow
        };

        var postulacionCreada = await _repositorioPostulacion.AgregarAsync(postulacion);

        // Recargar la postulacion con las relaciones para el mapeo
        var postulacionConDatos = await _repositorioPostulacion.ObtenerPorIdAsync(postulacionCreada.Id);
        return _mapper.Map<PostulacionDto>(postulacionConDatos!);
    }

    // Obtiene todas las postulaciones realizadas por un joven especifico
    public async Task<IEnumerable<PostulacionDto>> ObtenerPorJovenAsync(int jovenId)
    {
        var postulaciones = await _repositorioPostulacion.ObtenerPorJovenAsync(jovenId);
        return _mapper.Map<IEnumerable<PostulacionDto>>(postulaciones);
    }

    // Obtiene todas las postulaciones recibidas para una oferta de trabajo especifica
    public async Task<IEnumerable<PostulacionDto>> ObtenerPorOfertaAsync(int ofertaTrabajoId)
    {
        var postulaciones = await _repositorioPostulacion.ObtenerPorOfertaAsync(ofertaTrabajoId);
        return _mapper.Map<IEnumerable<PostulacionDto>>(postulaciones);
    }

    // Obtiene una postulacion por su identificador unico
    public async Task<PostulacionDto?> ObtenerPorIdAsync(int id)
    {
        var postulacion = await _repositorioPostulacion.ObtenerPorIdAsync(id);
        return postulacion is null ? null : _mapper.Map<PostulacionDto>(postulacion);
    }

    // Obtiene el conjunto de identificadores de habilidades del curriculum del joven.
    // Retorna un conjunto vacio si el joven no tiene curriculum o no tiene habilidades.
    private static HashSet<int> ObtenerHabilidadesDelCurriculum(Joven joven)
    {
        if (joven.Curriculum?.CurriculumHabilidades is null)
        {
            return new HashSet<int>();
        }

        return joven.Curriculum.CurriculumHabilidades
            .Select(ch => ch.HabilidadId)
            .ToHashSet();
    }

    // Busca cursos disponibles para cada requisito no relevante faltante.
    // Si existe mas de un curso para una habilidad, selecciona el mas corto (menor duracion).
    private async Task<List<CursoSugeridoDto>> ObtenerCursosSugeridosAsync(
        List<Requisito> requisitosNoRelevantesFaltantes)
    {
        var cursosSugeridos = new List<CursoSugeridoDto>();

        foreach (var requisito in requisitosNoRelevantesFaltantes)
        {
            // Obtener cursos disponibles para la habilidad requerida
            var cursosDisponibles = await _repositorioCurso
                .ObtenerPorHabilidadAsync(requisito.HabilidadId);

            // Seleccionar el curso mas corto para minimizar el tiempo de preparacion
            var cursoSeleccionado = cursosDisponibles
                .Where(c => c.Activo)
                .OrderBy(c => c.DuracionHoras)
                .FirstOrDefault();

            if (cursoSeleccionado is not null)
            {
                cursosSugeridos.Add(new CursoSugeridoDto
                {
                    CursoId = cursoSeleccionado.Id,
                    TituloCurso = cursoSeleccionado.Titulo,
                    HabilidadId = requisito.HabilidadId,
                    NombreHabilidad = requisito.Habilidad.Nombre,
                    DuracionHoras = cursoSeleccionado.DuracionHoras,
                    UrlMaterial = cursoSeleccionado.UrlMaterial,
                    DescripcionRequisito = requisito.Descripcion
                });
            }
            else
            {
                // No hay curso disponible: informar la habilidad faltante de todos modos
                cursosSugeridos.Add(new CursoSugeridoDto
                {
                    CursoId = 0,
                    TituloCurso = "Sin curso disponible",
                    HabilidadId = requisito.HabilidadId,
                    NombreHabilidad = requisito.Habilidad.Nombre,
                    DuracionHoras = 0,
                    UrlMaterial = null,
                    DescripcionRequisito = requisito.Descripcion
                });
            }
        }

        return cursosSugeridos;
    }
}
