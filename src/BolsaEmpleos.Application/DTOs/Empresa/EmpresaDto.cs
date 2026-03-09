namespace BolsaEmpleos.Application.DTOs.Empresa;

// DTO de respuesta con los datos publicos de una empresa.
// No expone informacion sensible como la contrasena.
public class EmpresaDto
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Descripcion { get; set; }
    public string? SitioWeb { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
}
