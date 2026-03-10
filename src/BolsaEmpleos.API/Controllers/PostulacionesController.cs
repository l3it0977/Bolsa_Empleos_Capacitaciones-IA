using BolsaEmpleos.Application.DTOs.Postulacion;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de postulaciones a ofertas de trabajo.
// Expone los endpoints del recurso /api/postulaciones.
[ApiController]
[Route("api/[controller]")]
public class PostulacionesController : ControllerBase
{
    private readonly IServicioPostulacion _servicioPostulacion;

    public PostulacionesController(IServicioPostulacion servicioPostulacion)
    {
        _servicioPostulacion = servicioPostulacion;
    }

    // GET api/postulaciones/{id} - Obtiene una postulacion por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PostulacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var postulacion = await _servicioPostulacion.ObtenerPorIdAsync(id);
        if (postulacion is null) return NotFound();
        return Ok(postulacion);
    }

    // GET api/postulaciones/joven/{jovenId} - Obtiene todas las postulaciones de un joven
    [HttpGet("joven/{jovenId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PostulacionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorJoven(int jovenId)
    {
        var postulaciones = await _servicioPostulacion.ObtenerPorJovenAsync(jovenId);
        return Ok(postulaciones);
    }

    // GET api/postulaciones/oferta/{ofertaTrabajoId} - Obtiene todas las postulaciones de una oferta
    [HttpGet("oferta/{ofertaTrabajoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PostulacionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorOferta(int ofertaTrabajoId)
    {
        var postulaciones = await _servicioPostulacion.ObtenerPorOfertaAsync(ofertaTrabajoId);
        return Ok(postulaciones);
    }

    // GET api/postulaciones/evaluar/joven/{jovenId}/oferta/{ofertaTrabajoId}
    // Evalua si un joven puede postularse a una oferta sin registrar la postulacion.
    // Retorna el detalle de brechas de habilidades y cursos sugeridos.
    [HttpGet("evaluar/joven/{jovenId:int}/oferta/{ofertaTrabajoId:int}")]
    [ProducesResponseType(typeof(ResultadoEvaluacionPostulacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EvaluarPostulacion(int jovenId, int ofertaTrabajoId)
    {
        try
        {
            var resultado = await _servicioPostulacion.EvaluarPostulacionAsync(jovenId, ofertaTrabajoId);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // POST api/postulaciones/joven/{jovenId}/oferta/{ofertaTrabajoId}
    // Registra la postulacion del joven si cumple con todos los requisitos de la oferta.
    [HttpPost("joven/{jovenId:int}/oferta/{ofertaTrabajoId:int}")]
    [ProducesResponseType(typeof(PostulacionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Postular(int jovenId, int ofertaTrabajoId)
    {
        try
        {
            var postulacion = await _servicioPostulacion.PostularAsync(jovenId, ofertaTrabajoId);
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = postulacion.Id },
                postulacion);
        }
        catch (InvalidOperationException ex)
        {
            // Retorna 409 Conflict cuando ya existe postulacion o el joven no puede postularse
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PATCH api/postulaciones/{id}/estado - Actualiza el estado de una postulacion (feedback de empresa)
    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(typeof(PostulacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] EstadoPostulacion nuevoEstado)
    {
        var postulacion = await _servicioPostulacion.ActualizarEstadoAsync(id, nuevoEstado);
        if (postulacion is null) return NotFound();
        return Ok(postulacion);
    }
}
