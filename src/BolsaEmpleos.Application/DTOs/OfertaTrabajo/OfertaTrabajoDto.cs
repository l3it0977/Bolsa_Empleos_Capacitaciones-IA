using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.OfertaTrabajo;

// DTO de respuesta con los datos de una oferta de trabajo.
public class OfertaTrabajoDto
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string RazonSocialEmpresa { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public decimal? Salario { get; set; }
    public EstadoOferta Estado { get; set; }
    public DateTime? FechaCierre { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Lista de requisitos de la oferta
    public IEnumerable<RequisitoDto> Requisitos { get; set; } = new List<RequisitoDto>();
}

// DTO interno para representar un requisito dentro de la oferta
public class RequisitoDto
{
    public int Id { get; set; }
    public int HabilidadId { get; set; }
    public string NombreHabilidad { get; set; } = string.Empty;
    public TipoRequisito TipoRequisito { get; set; }
    public string? Descripcion { get; set; }
}
