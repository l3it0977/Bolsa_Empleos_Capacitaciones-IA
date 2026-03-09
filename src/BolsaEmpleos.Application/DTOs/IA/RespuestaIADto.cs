namespace BolsaEmpleos.Application.DTOs.IA;

// DTO que representa la respuesta del joven a una pregunta de la evaluacion.
public class RespuestaIADto
{
    // Numero de la pregunta respondida (debe coincidir con PreguntaIADto.Numero)
    public int NumeroPregunta { get; set; }

    // Respuesta proporcionada por el joven
    public string Respuesta { get; set; } = string.Empty;
}
