using BolsaEmpleos.Application.DTOs.OfertaTrabajo;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de ofertas de trabajo.
// Expone los endpoints del recurso /api/ofertas-trabajo.
[ApiController]
[Route("api/ofertas-trabajo")]
public class OfertasTrabajoController : ControllerBase
{
    private readonly IServicioOfertaTrabajo _servicioOferta;

    public OfertasTrabajoController(IServicioOfertaTrabajo servicioOferta)
    {
        _servicioOferta = servicioOferta;
    }

    // GET api/ofertas-trabajo - Obtiene todas las ofertas publicadas
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfertaTrabajoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPublicadas()
    {
        var ofertas = await _servicioOferta.ObtenerPublicadasAsync();
        return Ok(ofertas);
    }

    // GET api/ofertas-trabajo/{id} - Obtiene una oferta especifica
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OfertaTrabajoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var oferta = await _servicioOferta.ObtenerPorIdAsync(id);
        if (oferta is null) return NotFound();
        return Ok(oferta);
    }

    // GET api/ofertas-trabajo/empresa/{empresaId} - Obtiene las ofertas de una empresa
    [HttpGet("empresa/{empresaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaTrabajoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorEmpresa(int empresaId)
    {
        var ofertas = await _servicioOferta.ObtenerPorEmpresaAsync(empresaId);
        return Ok(ofertas);
    }

    // POST api/ofertas-trabajo/empresa/{empresaId} - Crea una nueva oferta para una empresa
    [HttpPost("empresa/{empresaId:int}")]
    [ProducesResponseType(typeof(OfertaTrabajoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Crear(int empresaId, [FromBody] CrearOfertaTrabajoDto dto)
    {
        try
        {
            var oferta = await _servicioOferta.CrearAsync(empresaId, dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = oferta.Id }, oferta);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // PATCH api/ofertas-trabajo/{id}/estado - Cambia el estado de una oferta
    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoOferta nuevoEstado)
    {
        var actualizado = await _servicioOferta.CambiarEstadoAsync(id, nuevoEstado);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    // DELETE api/ofertas-trabajo/{id} - Elimina logicamente una oferta
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _servicioOferta.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
