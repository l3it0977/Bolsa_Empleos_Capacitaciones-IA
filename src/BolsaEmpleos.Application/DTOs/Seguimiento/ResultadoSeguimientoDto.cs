namespace BolsaEmpleos.Application.DTOs.Seguimiento;

// DTO de respuesta con el estado de seguimiento laboral de un joven.
// Resume su historial de postulaciones y, si no consiguio empleo,
// incluye una lista de ofertas recomendadas ordenadas por compatibilidad.
public class ResultadoSeguimientoDto
{
    // Identificador del joven cuyo seguimiento se consulta
    public int JovenId { get; set; }

    // Nombre completo del joven
    public string NombreJoven { get; set; } = string.Empty;

    // Indica si el joven tiene al menos una postulacion aceptada (consiguio empleo)
    public bool TieneEmpleoActual { get; set; }

    // Total de postulaciones registradas por el joven
    public int TotalPostulaciones { get; set; }

    // Cantidad de postulaciones aceptadas por empresas
    public int PostulacionesAceptadas { get; set; }

    // Cantidad de postulaciones rechazadas por empresas
    public int PostulacionesRechazadas { get; set; }

    // Cantidad de postulaciones que aun estan pendientes de respuesta
    public int PostulacionesPendientes { get; set; }

    // Lista de ofertas recomendadas basadas en el analisis del CV del joven.
    // Solo se completa cuando el joven no tiene empleo activo.
    // Las ofertas estan ordenadas de mayor a menor compatibilidad.
    public IEnumerable<RecomendacionOfertaDto> OfertasRecomendadas { get; set; }
        = new List<RecomendacionOfertaDto>();
}
