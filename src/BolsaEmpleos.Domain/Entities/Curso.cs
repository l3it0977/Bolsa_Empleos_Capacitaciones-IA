using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Entities;

// Representa un curso corto de capacitacion (entre 1 y 3 horas) disponible
// en la plataforma. Al aprobar el curso, la habilidad asociada se agrega
// automaticamente al curriculum del joven.
public class Curso : EntidadBase
{
    // Titulo descriptivo del curso
    public string Titulo { get; set; } = string.Empty;

    // Descripcion del contenido, objetivos y metodologia del curso
    public string Descripcion { get; set; } = string.Empty;

    // Identificador de la habilidad que se adquiere al aprobar el curso (clave foranea)
    public int HabilidadId { get; set; }

    // Habilidad que el joven adquiere al completar y aprobar el curso
    public Habilidad Habilidad { get; set; } = null!;

    // Duracion del curso expresada en horas (rango valido: 1 a 3 horas)
    public decimal DuracionHoras { get; set; }

    // URL o ruta al material del curso (video, PDF, plataforma externa)
    public string? UrlMaterial { get; set; }

    // Puntaje minimo (sobre 100) que debe obtener el joven para aprobar
    public int PuntajeMinimAprobacion { get; set; } = 70;

    // Evaluaciones de jovenes que han tomado este curso (relacion uno a muchos)
    public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
}
