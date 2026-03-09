namespace BolsaEmpleos.Infrastructure.IA;

// Configuracion para la conexion con el servicio de NotebookLM como motor de IA.
// Se vincula a la seccion "NotebookLM" del archivo appsettings.json.
public class ConfiguracionNotebookLM
{
    // URL base de la API de NotebookLM (p. ej. https://notebooklm.googleapis.com)
    public string UrlBase { get; set; } = string.Empty;

    // Clave de API para autenticar las solicitudes a NotebookLM
    public string ClaveApi { get; set; } = string.Empty;

    // Tiempo maximo de espera para cada solicitud, expresado en segundos
    public int TiempoEsperaSegundos { get; set; } = 30;

    // Numero maximo de preguntas que la IA puede generar por evaluacion
    public int MaximoPreguntas { get; set; } = 10;
}
