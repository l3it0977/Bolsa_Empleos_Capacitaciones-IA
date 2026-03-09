using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Evaluacion;

// DTO utilizado para registrar el resultado de una evaluacion de curso.
public class RegistrarResultadoEvaluacionDto
{
    // Puntaje obtenido por el joven en la evaluacion (entre 0 y 100)
    [Required(ErrorMessage = "El puntaje obtenido es obligatorio.")]
    [Range(0, 100, ErrorMessage = "El puntaje debe estar entre 0 y 100.")]
    public int PuntajeObtenido { get; set; }
}
