using BolsaEmpleos.Application.DTOs.Curriculum;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [Authorize(Roles = "Joven")]
    [ProducesResponseType(typeof(CurriculumDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorJoven(int jovenId)
    {
        if (!UsuarioCorrespondeAJoven(jovenId)) return Forbid();
        var curriculum = await _servicioCurriculum.ObtenerPorJovenAsync(jovenId);
        if (curriculum is null) return NotFound();
        return Ok(curriculum);
    }

    // PUT api/jovenes/{jovenId}/curriculum - Crea o actualiza el curriculum del joven
    [HttpPut]
    [Authorize(Roles = "Joven")]
    [ProducesResponseType(typeof(CurriculumDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Guardar(int jovenId, [FromBody] GuardarCurriculumDto dto)
    {
        if (!UsuarioCorrespondeAJoven(jovenId)) return Forbid();
        try
        {
            var curriculum = await _servicioCurriculum.GuardarAsync(jovenId, dto);
            return Ok(curriculum);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST api/jovenes/{jovenId}/curriculum/habilidades/{habilidadId} - Agrega una habilidad manual
    [HttpPost("habilidades/{habilidadId:int}")]
    [Authorize(Roles = "Joven")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AgregarHabilidad(int jovenId, int habilidadId)
    {
        if (!UsuarioCorrespondeAJoven(jovenId)) return Forbid();
        // Habilidad declarada manualmente (no obtenida por curso)
        var agregada = await _servicioCurriculum.AgregarHabilidadAsync(jovenId, habilidadId, obtenidaPorCurso: false);
        if (!agregada)
        {
            return Conflict(new { message = "La habilidad ya existe en el curriculum o el curriculum no fue encontrado." });
        }
        return NoContent();
    }

    private bool UsuarioCorrespondeAJoven(int jovenId)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var idToken) && idToken == jovenId;
    }
}
