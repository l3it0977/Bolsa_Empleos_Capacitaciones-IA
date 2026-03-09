namespace BolsaEmpleos.Application.DTOs.Curso;

// DTO de respuesta con los datos de un curso de capacitacion.
public class CursoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int HabilidadId { get; set; }
    public string NombreHabilidad { get; set; } = string.Empty;
    public decimal DuracionHoras { get; set; }
    public string? UrlMaterial { get; set; }
    public int PuntajeMinimAprobacion { get; set; }
    public bool Activo { get; set; }
}
