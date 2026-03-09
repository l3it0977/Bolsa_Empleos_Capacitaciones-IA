using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Entities;

// Representa la postulacion de un joven a una oferta de trabajo.
// Solo se permite crear una postulacion cuando el joven cumple con todos
// los requisitos de la oferta (relevantes y no relevantes).
public class Postulacion : EntidadBase
{
    // Identificador del joven que realiza la postulacion
    public int JovenId { get; set; }

    // Joven que postula (propiedad de navegacion)
    public Joven Joven { get; set; } = null!;

    // Identificador de la oferta de trabajo a la que se postula
    public int OfertaTrabajoId { get; set; }

    // Oferta de trabajo a la que se postula (propiedad de navegacion)
    public OfertaTrabajo OfertaTrabajo { get; set; } = null!;

    // Estado actual de la postulacion
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;

    // Fecha en que se registro la postulacion
    public DateTime FechaPostulacion { get; set; } = DateTime.UtcNow;
}
