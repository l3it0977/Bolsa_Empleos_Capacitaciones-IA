using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Entities;

// Tabla intermedia que relaciona un Curriculum con las Habilidades que contiene.
// Registra ademas si la habilidad fue adquirida a traves de la aprobacion
// de un curso en la plataforma o fue declarada manualmente por el joven.
public class CurriculumHabilidad : EntidadBase
{
    // Identificador del curriculum al que pertenece esta asociacion
    public int CurriculumId { get; set; }

    // Curriculum al que pertenece esta asociacion
    public Curriculum Curriculum { get; set; } = null!;

    // Identificador de la habilidad asociada
    public int HabilidadId { get; set; }

    // Habilidad asociada al curriculum
    public Habilidad Habilidad { get; set; } = null!;

    // Indica si esta habilidad se agrego como resultado de aprobar un curso
    public bool ObtenidaPorCurso { get; set; } = false;

    // Fecha en que se agrego la habilidad al curriculum
    public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;
}
