using BolsaEmpleos.Application.DTOs.Habilidad;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de habilidades del sistema.
// Expone los endpoints CRUD del recurso /api/habilidades.
[ApiController]
[Route("api/[controller]")]
public class HabilidadesController : ControllerBase
{
    private readonly IServicioHabilidad _servicioHabilidad;

    public HabilidadesController(IServicioHabilidad servicioHabilidad)
    {
        _servicioHabilidad = servicioHabilidad;
    }

    // GET api/habilidades - Obtiene todas las habilidades activas
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HabilidadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodas()
    {
        var habilidades = await _servicioHabilidad.ObtenerTodosAsync();
        return Ok(habilidades);
    }

    // GET api/habilidades/{id} - Obtiene una habilidad por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(HabilidadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var habilidad = await _servicioHabilidad.ObtenerPorIdAsync(id);
        if (habilidad is null) return NotFound();
        return Ok(habilidad);
    }

    // GET api/habilidades/buscar?nombre={nombre} - Busca habilidades por nombre
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<HabilidadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> BuscarPorNombre([FromQuery] string nombre)
    {
        var habilidades = await _servicioHabilidad.BuscarPorNombreAsync(nombre);
        return Ok(habilidades);
    }

    // POST api/habilidades - Crea una nueva habilidad
    [HttpPost]
    [ProducesResponseType(typeof(HabilidadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear([FromBody] GuardarHabilidadDto dto)
    {
        try
        {
            var habilidad = await _servicioHabilidad.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = habilidad.Id }, habilidad);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/habilidades/{id} - Actualiza una habilidad existente
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(HabilidadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] GuardarHabilidadDto dto)
    {
        var habilidad = await _servicioHabilidad.ActualizarAsync(id, dto);
        if (habilidad is null) return NotFound();
        return Ok(habilidad);
    }

    // DELETE api/habilidades/{id} - Elimina logicamente una habilidad
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _servicioHabilidad.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
