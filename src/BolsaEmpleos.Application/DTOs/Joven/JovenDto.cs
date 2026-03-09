using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.Joven;

// DTO de respuesta con los datos publicos de un joven.
// No expone informacion sensible como la contrasena.
public class JovenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public NivelEducativo NivelEducativo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
}
