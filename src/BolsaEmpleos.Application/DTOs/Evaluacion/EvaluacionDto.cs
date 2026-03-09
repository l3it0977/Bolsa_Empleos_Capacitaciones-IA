using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.Evaluacion;

// DTO de respuesta con los datos de una evaluacion de curso.
public class EvaluacionDto
{
    public int Id { get; set; }
    public int JovenId { get; set; }
    public string NombreJoven { get; set; } = string.Empty;
    public int CursoId { get; set; }
    public string TituloCurso { get; set; } = string.Empty;
    public int? PuntajeObtenido { get; set; }
    public EstadoEvaluacion Estado { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int Intentos { get; set; }
    public DateTime FechaCreacion { get; set; }
}
