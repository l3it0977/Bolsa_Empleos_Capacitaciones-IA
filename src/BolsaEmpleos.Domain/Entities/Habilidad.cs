using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Entities;

// Representa una habilidad o competencia que puede poseer un joven,
// ser requerida por una oferta de trabajo o ser ensenada en un curso.
// Actua como el nodo central del modelo de brechas de habilidades.
public class Habilidad : EntidadBase
{
    // Nombre de la habilidad (ejemplo: "Excel", "Comunicacion efectiva", "Python")
    public string Nombre { get; set; } = string.Empty;

    // Descripcion detallada de lo que implica esta habilidad
    public string? Descripcion { get; set; }

    // Categoria o area de conocimiento a la que pertenece la habilidad
    public string? Categoria { get; set; }

    // Cursos que ensenan esta habilidad (relacion uno a muchos)
    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    // Requisitos de ofertas que solicitan esta habilidad (relacion uno a muchos)
    public ICollection<Requisito> Requisitos { get; set; } = new List<Requisito>();

    // Asociaciones de curricula que incluyen esta habilidad (relacion uno a muchos)
    public ICollection<CurriculumHabilidad> CurriculumHabilidades { get; set; } = new List<CurriculumHabilidad>();
}
