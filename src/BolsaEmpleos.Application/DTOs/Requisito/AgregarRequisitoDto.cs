using System.ComponentModel.DataAnnotations;

namespace BolsaEmpleos.Application.DTOs.Requisito;

// DTO utilizado para agregar un requisito a una oferta de trabajo existente.
public class AgregarRequisitoDto
{
    [Required(ErrorMessage = "El identificador de la habilidad es obligatorio.")]
    public int HabilidadId { get; set; }

    [Required(ErrorMessage = "El tipo de requisito es obligatorio.")]
    public int TipoRequisito { get; set; }

    [MaxLength(500, ErrorMessage = "La descripcion no puede superar 500 caracteres.")]
    public string? Descripcion { get; set; }
}
