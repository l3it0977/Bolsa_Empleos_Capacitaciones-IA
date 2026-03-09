using BolsaEmpleos.Application.DTOs.Evaluacion;
using BolsaEmpleos.Application.DTOs.IA;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que orquesta la evaluacion de cursos con soporte de inteligencia artificial.
// Utiliza NotebookLM (a traves de IClienteIA) para generar preguntas y evaluar respuestas.
// Al aprobar, delega en ServicioEvaluacion para registrar el resultado y agregar la habilidad al CV.
public class ServicioEvaluacionIA : IServicioEvaluacionIA
{
    private readonly IClienteIA _clienteIA;
    private readonly IServicioEvaluacion _servicioEvaluacion;
    private readonly IRepositorioEvaluacion _repositorioEvaluacion;
    private readonly IRepositorioCurso _repositorioCurso;
    private readonly IRepositorioHabilidad _repositorioHabilidad;

    public ServicioEvaluacionIA(
        IClienteIA clienteIA,
        IServicioEvaluacion servicioEvaluacion,
        IRepositorioEvaluacion repositorioEvaluacion,
        IRepositorioCurso repositorioCurso,
        IRepositorioHabilidad repositorioHabilidad)
    {
        _clienteIA = clienteIA;
        _servicioEvaluacion = servicioEvaluacion;
        _repositorioEvaluacion = repositorioEvaluacion;
        _repositorioCurso = repositorioCurso;
        _repositorioHabilidad = repositorioHabilidad;
    }

    // Genera las preguntas de evaluacion para un curso dado usando la IA.
    // La IA analiza el titulo y la descripcion del curso para formular preguntas relevantes.
    public async Task<List<PreguntaIADto>> GenerarPreguntasAsync(int cursoId, int numeroPreguntas = 5)
    {
        // Obtener el curso para tener el contenido que la IA analizara
        var curso = await _repositorioCurso.ObtenerPorIdAsync(cursoId);
        if (curso is null || !curso.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un curso activo con el identificador {cursoId}.");
        }

        // Delegar en la IA la generacion de preguntas basadas en el contenido del curso
        var preguntas = await _clienteIA.GenerarPreguntasAsync(
            curso.Titulo,
            curso.Descripcion,
            numeroPreguntas);

        return preguntas;
    }

    // Evalua las respuestas del joven usando la IA.
    // Flujo:
    //   1. Recupera la evaluacion activa y el curso correspondiente.
    //   2. Solicita a la IA que valore las respuestas del joven.
    //   3. Registra el resultado en el backend (lo que tambien agrega la habilidad al CV si aprobo).
    //   4. Retorna el resultado con retroalimentacion y la habilidad obtenida (si corresponde).
    public async Task<ResultadoEvaluacionIADto> EvaluarRespuestasAsync(
        SolicitudEvaluarRespuestasDto solicitud)
    {
        // Recuperar la evaluacion activa
        var evaluacion = await _repositorioEvaluacion.ObtenerPorIdAsync(solicitud.EvaluacionId);
        if (evaluacion is null)
        {
            throw new InvalidOperationException(
                $"No se encontro la evaluacion con el identificador {solicitud.EvaluacionId}.");
        }

        // Obtener el curso para conocer el puntaje minimo y el contenido
        var curso = await _repositorioCurso.ObtenerPorIdAsync(evaluacion.CursoId);
        if (curso is null)
        {
            throw new InvalidOperationException(
                $"No se encontro el curso con el identificador {evaluacion.CursoId}.");
        }

        // Usar las preguntas originales que se presentaron al joven
        // para garantizar coherencia entre lo que respondio y lo que se evalua
        var preguntas = solicitud.Preguntas;

        // Evaluar las respuestas del joven usando la IA
        var (puntaje, retroalimentacion, feedback) = await _clienteIA.EvaluarRespuestasAsync(
            curso.Titulo,
            preguntas,
            solicitud.Respuestas);

        // Registrar el resultado en el backend.
        // ServicioEvaluacion se encarga de agregar la habilidad al CV si el joven aprobo.
        var registrarDto = new RegistrarResultadoEvaluacionDto { PuntajeObtenido = puntaje };
        await _servicioEvaluacion.RegistrarResultadoAsync(solicitud.EvaluacionId, registrarDto);

        // Determinar si el joven aprobo segun el puntaje minimo del curso
        var aprobo = puntaje >= curso.PuntajeMinimoAprobacion;

        // Obtener el nombre de la habilidad para incluirlo en el resultado (si aprobo)
        string? nombreHabilidad = null;
        if (aprobo)
        {
            var habilidad = await _repositorioHabilidad.ObtenerPorIdAsync(curso.HabilidadId);
            nombreHabilidad = habilidad?.Nombre;
        }

        return new ResultadoEvaluacionIADto
        {
            EvaluacionId = solicitud.EvaluacionId,
            PuntajeObtenido = puntaje,
            PuntajeMinimo = curso.PuntajeMinimoAprobacion,
            Aprobo = aprobo,
            Retroalimentacion = retroalimentacion,
            FeedbackPreguntas = feedback,
            HabilidadAgregada = nombreHabilidad
        };
    }
}
