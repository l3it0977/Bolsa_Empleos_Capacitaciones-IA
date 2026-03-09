using BolsaEmpleos.Application.DTOs.Joven;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de jovenes en la plataforma.
// Expone los endpoints CRUD del recurso /api/jovenes.
[ApiController]
[Route("api/[controller]")]
public class JovenesController : ControllerBase
{
    private readonly IServicioJoven _servicioJoven;

    public JovenesController(IServicioJoven servicioJoven)
    {
        _servicioJoven = servicioJoven;
    }

    // GET api/jovenes - Obtiene todos los jovenes activos
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JovenDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var jovenes = await _servicioJoven.ObtenerTodosAsync();
        return Ok(jovenes);
    }

    // GET api/jovenes/{id} - Obtiene un joven por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(JovenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var joven = await _servicioJoven.ObtenerPorIdAsync(id);
        if (joven is null) return NotFound();
        return Ok(joven);
    }

    // POST api/jovenes - Registra un nuevo joven
    [HttpPost]
    [ProducesResponseType(typeof(JovenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear([FromBody] CrearJovenDto dto)
    {
        try
        {
            var joven = await _servicioJoven.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = joven.Id }, joven);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/jovenes/{id} - Actualiza los datos de un joven existente
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(JovenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarJovenDto dto)
    {
        var joven = await _servicioJoven.ActualizarAsync(id, dto);
        if (joven is null) return NotFound();
        return Ok(joven);
    }

    // DELETE api/jovenes/{id} - Elimina logicamente un joven
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _servicioJoven.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
