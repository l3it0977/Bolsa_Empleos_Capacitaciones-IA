namespace BolsaEmpleos.Application.DTOs.Habilidad;

// DTO de respuesta con los datos de una habilidad.
public class HabilidadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public bool Activo { get; set; }
}
