using BolsaEmpleos.Application.DTOs.Curso;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de cursos de capacitacion.
// Expone los endpoints CRUD del recurso /api/cursos.
[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private readonly IServicioCurso _servicioCurso;

    public CursosController(IServicioCurso servicioCurso)
    {
        _servicioCurso = servicioCurso;
    }

    // GET api/cursos - Obtiene todos los cursos activos
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var cursos = await _servicioCurso.ObtenerTodosAsync();
        return Ok(cursos);
    }

    // GET api/cursos/{id} - Obtiene un curso por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var curso = await _servicioCurso.ObtenerPorIdAsync(id);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    // GET api/cursos/habilidad/{habilidadId} - Obtiene cursos de una habilidad especifica
    [HttpGet("habilidad/{habilidadId:int}")]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorHabilidad(int habilidadId)
    {
        var cursos = await _servicioCurso.ObtenerPorHabilidadAsync(habilidadId);
        return Ok(cursos);
    }

    // POST api/cursos - Crea un nuevo curso de capacitacion
    [HttpPost]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Crear([FromBody] GuardarCursoDto dto)
    {
        try
        {
            var curso = await _servicioCurso.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = curso.Id }, curso);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    // PUT api/cursos/{id} - Actualiza un curso existente
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] GuardarCursoDto dto)
    {
        var curso = await _servicioCurso.ActualizarAsync(id, dto);
        if (curso is null) return NotFound();
        return Ok(curso);
    }

    // DELETE api/cursos/{id} - Elimina logicamente un curso
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _servicioCurso.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
