using BolsaEmpleos.Application.DTOs.Seguimiento;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para el seguimiento laboral de jovenes y la recomendacion de empleos.
// Expone los endpoints del recurso /api/seguimiento.
[ApiController]
[Route("api/[controller]")]
public class SeguimientoController : ControllerBase
{
    private readonly IServicioSeguimiento _servicioSeguimiento;

    public SeguimientoController(IServicioSeguimiento servicioSeguimiento)
    {
        _servicioSeguimiento = servicioSeguimiento;
    }

    // GET api/seguimiento/joven/{jovenId}
    // Obtiene el resumen completo de seguimiento laboral del joven:
    // estado de empleo, historial de postulaciones y ofertas recomendadas si no consiguio empleo.
    [HttpGet("joven/{jovenId:int}")]
    [ProducesResponseType(typeof(ResultadoSeguimientoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerSeguimiento(int jovenId)
    {
        try
        {
            var resultado = await _servicioSeguimiento.ObtenerSeguimientoAsync(jovenId);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // GET api/seguimiento/recomendaciones/joven/{jovenId}
    // Aplica el algoritmo de parentesco para obtener ofertas compatibles con el CV del joven,
    // ordenadas de mayor a menor porcentaje de compatibilidad.
    [HttpGet("recomendaciones/joven/{jovenId:int}")]
    [ProducesResponseType(typeof(IEnumerable<RecomendacionOfertaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerRecomendaciones(int jovenId)
    {
        try
        {
            var recomendaciones = await _servicioSeguimiento.RecomendarOfertasAsync(jovenId);
            return Ok(recomendaciones);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // PATCH api/seguimiento/postulacion/{postulacionId}/resultado
    // Registra si el joven consiguio empleo mediante la postulacion indicada.
    // El parametro consiguioEmpleo (bool en el body) determina el resultado:
    // true -> estado Aceptada, false -> estado Rechazada.
    [HttpPatch("postulacion/{postulacionId:int}/resultado")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarResultadoEmpleo(
        int postulacionId, [FromBody] bool consiguioEmpleo)
    {
        var postulacion = await _servicioSeguimiento
            .RegistrarResultadoEmpleoAsync(postulacionId, consiguioEmpleo);

        if (postulacion is null)
        {
            return NotFound(new { mensaje = $"No se encontro la postulacion con identificador {postulacionId}." });
        }

        return Ok(postulacion);
    }
}
