using BolsaEmpleos.Application.DTOs.Evaluacion;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de evaluaciones de cursos.
// Expone los endpoints del recurso /api/evaluaciones.
[ApiController]
[Route("api/[controller]")]
public class EvaluacionesController : ControllerBase
{
    private readonly IServicioEvaluacion _servicioEvaluacion;

    public EvaluacionesController(IServicioEvaluacion servicioEvaluacion)
    {
        _servicioEvaluacion = servicioEvaluacion;
    }

    // GET api/evaluaciones/joven/{jovenId} - Obtiene todas las evaluaciones de un joven
    [HttpGet("joven/{jovenId:int}")]
    [ProducesResponseType(typeof(IEnumerable<EvaluacionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorJoven(int jovenId)
    {
        var evaluaciones = await _servicioEvaluacion.ObtenerPorJovenAsync(jovenId);
        return Ok(evaluaciones);
    }

    // GET api/evaluaciones/{id} - Obtiene una evaluacion por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EvaluacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var evaluacion = await _servicioEvaluacion.ObtenerPorIdAsync(id);
        if (evaluacion is null) return NotFound();
        return Ok(evaluacion);
    }

    // POST api/evaluaciones/joven/{jovenId}/curso/{cursoId} - Inicia una evaluacion
    [HttpPost("joven/{jovenId:int}/curso/{cursoId:int}")]
    [ProducesResponseType(typeof(EvaluacionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> IniciarEvaluacion(int jovenId, int cursoId)
    {
        try
        {
            var evaluacion = await _servicioEvaluacion.IniciarEvaluacionAsync(jovenId, cursoId);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = evaluacion.Id }, evaluacion);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PATCH api/evaluaciones/{id}/resultado - Registra el resultado de una evaluacion
    [HttpPatch("{id:int}/resultado")]
    [ProducesResponseType(typeof(EvaluacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarResultado(
        int id, [FromBody] RegistrarResultadoEvaluacionDto dto)
    {
        var evaluacion = await _servicioEvaluacion.RegistrarResultadoAsync(id, dto);
        if (evaluacion is null) return NotFound();
        return Ok(evaluacion);
    }
}
