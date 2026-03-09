using BolsaEmpleos.Application.DTOs.IA;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la evaluacion de cursos con inteligencia artificial (NotebookLM).
// Expone los endpoints del recurso /api/evaluacion-ia.
[ApiController]
[Route("api/evaluacion-ia")]
public class EvaluacionIAController : ControllerBase
{
    private readonly IServicioEvaluacionIA _servicioEvaluacionIA;

    public EvaluacionIAController(IServicioEvaluacionIA servicioEvaluacionIA)
    {
        _servicioEvaluacionIA = servicioEvaluacionIA;
    }

    // GET api/evaluacion-ia/cursos/{cursoId}/preguntas - Genera preguntas de evaluacion para un curso
    // La IA analiza el contenido del curso y genera preguntas relevantes para evaluar al joven.
    [HttpGet("cursos/{cursoId:int}/preguntas")]
    [ProducesResponseType(typeof(IEnumerable<PreguntaIADto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerarPreguntas(
        int cursoId,
        [FromQuery] int cantidad = 5)
    {
        if (cantidad < 1 || cantidad > 10)
        {
            return BadRequest(new
            {
                mensaje = "La cantidad de preguntas debe estar entre 1 y 10."
            });
        }

        try
        {
            var preguntas = await _servicioEvaluacionIA.GenerarPreguntasAsync(cursoId, cantidad);
            return Ok(preguntas);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // POST api/evaluacion-ia/evaluar - Evalua las respuestas del joven usando la IA
    // Si el puntaje supera el minimo del curso, la habilidad se agrega automaticamente al CV.
    [HttpPost("evaluar")]
    [ProducesResponseType(typeof(ResultadoEvaluacionIADto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EvaluarRespuestas(
        [FromBody] SolicitudEvaluarRespuestasDto solicitud)
    {
        try
        {
            var resultado = await _servicioEvaluacionIA.EvaluarRespuestasAsync(solicitud);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
