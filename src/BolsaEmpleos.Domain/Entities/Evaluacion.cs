using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Entities;

// Representa la evaluacion de un joven sobre un curso especifico.
// Al aprobar la evaluacion, la habilidad del curso se agrega al curriculum del joven.
// Este proceso es el mecanismo central de capacitacion de la plataforma.
public class Evaluacion : EntidadBase
{
    // Identificador del joven que realiza la evaluacion (clave foranea)
    public int JovenId { get; set; }

    // Joven que realiza esta evaluacion
    public Joven Joven { get; set; } = null!;

    // Identificador del curso evaluado (clave foranea)
    public int CursoId { get; set; }

    // Curso sobre el que se realiza la evaluacion
    public Curso Curso { get; set; } = null!;

    // Puntaje obtenido por el joven en la evaluacion (rango 0-100)
    public int? PuntajeObtenido { get; set; }

    // Estado actual de la evaluacion dentro de su ciclo de vida
    public EstadoEvaluacion Estado { get; set; } = EstadoEvaluacion.Pendiente;

    // Fecha y hora en que el joven inicio la evaluacion
    public DateTime? FechaInicio { get; set; }

    // Fecha y hora en que el joven finalizo la evaluacion
    public DateTime? FechaFin { get; set; }

    // Numero de intentos realizados para aprobar la evaluacion
    public int Intentos { get; set; } = 0;
}
