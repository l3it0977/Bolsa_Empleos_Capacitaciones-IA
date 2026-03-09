namespace BolsaEmpleos.Domain.Enums;

// Estados posibles de una postulacion a una oferta de trabajo.
public enum EstadoPostulacion
{
    // La postulacion fue registrada y esta en revision
    Pendiente = 0,

    // La empresa acepto al joven para el puesto
    Aceptada = 1,

    // La empresa rechazo la postulacion
    Rechazada = 2
}
