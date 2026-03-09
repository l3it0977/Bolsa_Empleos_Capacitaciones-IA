using System.ComponentModel.DataAnnotations;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.OfertaTrabajo;

// DTO utilizado para crear una nueva oferta de trabajo.
public class CrearOfertaTrabajoDto
{
    [Required(ErrorMessage = "El titulo de la oferta es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripcion de la oferta es obligatoria.")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ubicacion es obligatoria.")]
    [MaxLength(200, ErrorMessage = "La ubicacion no puede superar 200 caracteres.")]
    public string Ubicacion { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un valor positivo.")]
    public decimal? Salario { get; set; }

    public DateTime? FechaCierre { get; set; }

    // Requisitos de habilidades para la oferta
    public IEnumerable<CrearRequisitoDto> Requisitos { get; set; } = new List<CrearRequisitoDto>();
}

// DTO para agregar un requisito al crear una oferta
public class CrearRequisitoDto
{
    [Required(ErrorMessage = "El identificador de la habilidad es obligatorio.")]
    public int HabilidadId { get; set; }

    public TipoRequisito TipoRequisito { get; set; } = TipoRequisito.Relevante;

    [MaxLength(500)]
    public string? Descripcion { get; set; }
}
