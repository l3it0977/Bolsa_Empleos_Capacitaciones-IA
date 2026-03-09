using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Application.DTOs.Postulacion;

// DTO de respuesta con los datos de una postulacion registrada.
public class PostulacionDto
{
    // Identificador unico de la postulacion
    public int Id { get; set; }

    // Identificador del joven que realizo la postulacion
    public int JovenId { get; set; }

    // Nombre completo del joven
    public string NombreJoven { get; set; } = string.Empty;

    // Identificador de la oferta de trabajo
    public int OfertaTrabajoId { get; set; }

    // Titulo de la oferta de trabajo
    public string TituloOferta { get; set; } = string.Empty;

    // Estado actual de la postulacion
    public EstadoPostulacion Estado { get; set; }

    // Fecha en que se registro la postulacion
    public DateTime FechaPostulacion { get; set; }

    // Fecha de creacion del registro
    public DateTime FechaCreacion { get; set; }
}
