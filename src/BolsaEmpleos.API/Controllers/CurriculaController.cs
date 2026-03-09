using BolsaEmpleos.Application.DTOs.Curriculum;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de curricula de jovenes.
// Expone los endpoints del recurso /api/jovenes/{jovenId}/curriculum.
[ApiController]
[Route("api/jovenes/{jovenId:int}/curriculum")]
public class CurriculaController : ControllerBase
{
    private readonly IServicioCurriculum _servicioCurriculum;

    public CurriculaController(IServicioCurriculum servicioCurriculum)
    {
        _servicioCurriculum = servicioCurriculum;
    }

    // GET api/jovenes/{jovenId}/curriculum - Obtiene el curriculum de un joven
    [HttpGet]
    [ProducesResponseType(typeof(CurriculumDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorJoven(int jovenId)
    {
        var curriculum = await _servicioCurriculum.ObtenerPorJovenAsync(jovenId);
        if (curriculum is null) return NotFound();
        return Ok(curriculum);
    }

    // PUT api/jovenes/{jovenId}/curriculum - Crea o actualiza el curriculum del joven
    [HttpPut]
    [ProducesResponseType(typeof(CurriculumDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Guardar(int jovenId, [FromBody] GuardarCurriculumDto dto)
    {
        try
        {
            var curriculum = await _servicioCurriculum.GuardarAsync(jovenId, dto);
            return Ok(curriculum);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // POST api/jovenes/{jovenId}/curriculum/habilidades/{habilidadId} - Agrega una habilidad manual
    [HttpPost("habilidades/{habilidadId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AgregarHabilidad(int jovenId, int habilidadId)
    {
        // Habilidad declarada manualmente (no obtenida por curso)
        var agregada = await _servicioCurriculum.AgregarHabilidadAsync(jovenId, habilidadId, obtenidaPorCurso: false);
        if (!agregada)
        {
            return Conflict(new { mensaje = "La habilidad ya existe en el curriculum o el curriculum no fue encontrado." });
        }
        return NoContent();
    }
}
