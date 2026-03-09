using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Entities;

// Representa el curriculum vitae de un joven.
// Sus datos personales se heredan desde la entidad Joven para evitar
// duplicacion; ademas contiene habilidades y experiencias propias del CV.
public class Curriculum : EntidadBase
{
    // Identificador del joven dueno del curriculum (clave foranea)
    public int JovenId { get; set; }

    // Joven al que pertenece este curriculum (relacion uno a uno)
    public Joven Joven { get; set; } = null!;

    // Resumen profesional o perfil redactado por el joven
    public string? ResumenProfesional { get; set; }

    // Titulo profesional o cargo objetivo del joven
    public string? TituloProfesional { get; set; }

    // Enlace a portfolio o sitio web personal del joven
    public string? UrlPortfolio { get; set; }

    // Habilidades registradas en el curriculum (relacion muchos a muchos
    // a traves de la tabla intermedia CurriculumHabilidad)
    public ICollection<CurriculumHabilidad> CurriculumHabilidades { get; set; } = new List<CurriculumHabilidad>();
}
