namespace BolsaEmpleos.Application.DTOs.Curriculum;

// DTO de respuesta con los datos del curriculum de un joven.
public class CurriculumDto
{
    public int Id { get; set; }
    public int JovenId { get; set; }

    // Datos heredados del joven para presentar en el curriculum
    public string NombreJoven { get; set; } = string.Empty;
    public string ApellidoJoven { get; set; } = string.Empty;
    public string CorreoElectronicoJoven { get; set; } = string.Empty;

    public string? ResumenProfesional { get; set; }
    public string? TituloProfesional { get; set; }
    public string? UrlPortfolio { get; set; }

    // Lista de habilidades registradas en el curriculum
    public IEnumerable<HabilidadCurriculumDto> Habilidades { get; set; } = new List<HabilidadCurriculumDto>();
}

// DTO interno para representar una habilidad dentro del curriculum
public class HabilidadCurriculumDto
{
    public int HabilidadId { get; set; }
    public string NombreHabilidad { get; set; } = string.Empty;
    public string? Categoria { get; set; }

    // Indica si la habilidad fue adquirida aprobando un curso en la plataforma
    public bool ObtenidaPorCurso { get; set; }
    public DateTime FechaAgregado { get; set; }
}
