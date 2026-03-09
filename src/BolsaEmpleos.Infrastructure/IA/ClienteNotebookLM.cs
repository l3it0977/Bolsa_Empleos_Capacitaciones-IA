using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BolsaEmpleos.Application.DTOs.IA;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BolsaEmpleos.Infrastructure.IA;

// Implementacion del cliente de IA que se comunica con NotebookLM mediante HTTP.
// Envia el contenido del curso para generar preguntas y evaluar las respuestas del joven.
// Si la URL base no esta configurada, lanza una excepcion explicativa.
public class ClienteNotebookLM : IClienteIA
{
    private readonly HttpClient _httpClient;
    private readonly ConfiguracionNotebookLM _configuracion;
    private readonly ILogger<ClienteNotebookLM> _logger;

    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ClienteNotebookLM(
        HttpClient httpClient,
        IOptions<ConfiguracionNotebookLM> configuracion,
        ILogger<ClienteNotebookLM> logger)
    {
        _httpClient = httpClient;
        _configuracion = configuracion.Value;
        _logger = logger;
    }

    // Envia el contenido del curso a NotebookLM y recibe las preguntas generadas.
    // El modelo analiza titulo y descripcion para formular preguntas relevantes sobre el tema.
    public async Task<List<PreguntaIADto>> GenerarPreguntasAsync(
        string tituloCurso,
        string descripcionCurso,
        int numeroPreguntas = 5)
    {
        ValidarConfiguracion();

        _logger.LogInformation(
            "Generando {Cantidad} preguntas para el curso '{Titulo}' via NotebookLM.",
            numeroPreguntas, tituloCurso);

        // Construir la solicitud con el contenido del curso
        var solicitud = new
        {
            tituloCurso,
            descripcionCurso,
            numeroPreguntas,
            idioma = "es"
        };

        var respuesta = await EnviarSolicitudAsync<List<RespuestaPreguntaNotebookLM>>(
            "api/v1/generate-questions", solicitud);

        // Mapear la respuesta de NotebookLM al DTO del dominio
        return respuesta
            .Select((p, indice) => new PreguntaIADto
            {
                Numero = indice + 1,
                Enunciado = p.Enunciado,
                Tipo = p.Tipo,
                Opciones = p.Opciones ?? new List<string>()
            })
            .ToList();
    }

    // Envia las preguntas y respuestas del joven a NotebookLM para su evaluacion.
    // La IA determina cuales respuestas son correctas y calcula el puntaje final.
    public async Task<(int puntaje, string retroalimentacion, List<FeedbackPreguntaDto> feedback)>
        EvaluarRespuestasAsync(
            string tituloCurso,
            List<PreguntaIADto> preguntas,
            List<RespuestaIADto> respuestas)
    {
        ValidarConfiguracion();

        _logger.LogInformation(
            "Evaluando {Total} respuestas para el curso '{Titulo}' via NotebookLM.",
            respuestas.Count, tituloCurso);

        // Construir la solicitud con preguntas y respuestas del joven
        var solicitud = new
        {
            tituloCurso,
            preguntas = preguntas.Select(p => new
            {
                numero = p.Numero,
                enunciado = p.Enunciado,
                tipo = p.Tipo,
                opciones = p.Opciones
            }),
            respuestas = respuestas.Select(r => new
            {
                numeroPregunta = r.NumeroPregunta,
                respuesta = r.Respuesta
            })
        };

        var resultado = await EnviarSolicitudAsync<RespuestaEvaluacionNotebookLM>(
            "api/v1/evaluate-answers", solicitud);

        // Mapear el feedback detallado de NotebookLM al DTO del dominio
        var feedbackMapeado = resultado.FeedbackPreguntas?
            .Select(f => new FeedbackPreguntaDto
            {
                NumeroPregunta = f.NumeroPregunta,
                EsCorrecta = f.EsCorrecta,
                Explicacion = f.Explicacion ?? string.Empty
            })
            .ToList() ?? new List<FeedbackPreguntaDto>();

        return (resultado.Puntaje, resultado.Retroalimentacion ?? string.Empty, feedbackMapeado);
    }

    // Envio generico de solicitudes HTTP a la API de NotebookLM con autenticacion y JSON.
    private async Task<TRespuesta> EnviarSolicitudAsync<TRespuesta>(
        string ruta, object cuerpo)
    {
        var urlCompleta = $"{_configuracion.UrlBase.TrimEnd('/')}/{ruta}";
        var contenido = new StringContent(
            JsonSerializer.Serialize(cuerpo, OpcionesJson),
            Encoding.UTF8,
            "application/json");

        // Agregar la clave de API en el encabezado de la solicitud individual
        // para evitar condiciones de carrera en solicitudes concurrentes
        var mensaje = new HttpRequestMessage(HttpMethod.Post, urlCompleta)
        {
            Content = contenido
        };
        mensaje.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _configuracion.ClaveApi);

        try
        {
            var respuestaHttp = await _httpClient.SendAsync(mensaje);
            respuestaHttp.EnsureSuccessStatusCode();

            var json = await respuestaHttp.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<TRespuesta>(json, OpcionesJson);

            if (resultado is null)
            {
                throw new InvalidOperationException(
                    "La respuesta de NotebookLM no pudo ser deserializada.");
            }

            return resultado;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al comunicarse con NotebookLM en {Url}.", urlCompleta);
            throw new InvalidOperationException(
                "No se pudo comunicar con el servicio de inteligencia artificial. " +
                "Verifique la configuracion de NotebookLM.", ex);
        }
    }

    // Verifica que la configuracion de NotebookLM este definida antes de realizar solicitudes.
    private void ValidarConfiguracion()
    {
        if (string.IsNullOrWhiteSpace(_configuracion.UrlBase))
        {
            throw new InvalidOperationException(
                "La URL base de NotebookLM no esta configurada. " +
                "Configure la seccion 'NotebookLM:UrlBase' en appsettings.json.");
        }

        if (string.IsNullOrWhiteSpace(_configuracion.ClaveApi))
        {
            throw new InvalidOperationException(
                "La clave de API de NotebookLM no esta configurada. " +
                "Configure la seccion 'NotebookLM:ClaveApi' en appsettings.json.");
        }
    }

    // Modelos internos que representan la estructura JSON de respuesta de NotebookLM

    private sealed class RespuestaPreguntaNotebookLM
    {
        [JsonPropertyName("enunciado")]
        public string Enunciado { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = "opcion_multiple";

        [JsonPropertyName("opciones")]
        public List<string>? Opciones { get; set; }
    }

    private sealed class RespuestaEvaluacionNotebookLM
    {
        [JsonPropertyName("puntaje")]
        public int Puntaje { get; set; }

        [JsonPropertyName("retroalimentacion")]
        public string? Retroalimentacion { get; set; }

        [JsonPropertyName("feedbackPreguntas")]
        public List<FeedbackPreguntaNotebookLM>? FeedbackPreguntas { get; set; }
    }

    private sealed class FeedbackPreguntaNotebookLM
    {
        [JsonPropertyName("numeroPregunta")]
        public int NumeroPregunta { get; set; }

        [JsonPropertyName("esCorrecta")]
        public bool EsCorrecta { get; set; }

        [JsonPropertyName("explicacion")]
        public string? Explicacion { get; set; }
    }
}
