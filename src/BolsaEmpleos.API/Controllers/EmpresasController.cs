using BolsaEmpleos.Application.DTOs.Empresa;
using BolsaEmpleos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaEmpleos.API.Controllers;

// Controlador REST para la gestion de empresas en la plataforma.
// Expone los endpoints CRUD del recurso /api/empresas.
[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IServicioEmpresa _servicioEmpresa;

    public EmpresasController(IServicioEmpresa servicioEmpresa)
    {
        _servicioEmpresa = servicioEmpresa;
    }

    // GET api/empresas - Obtiene todas las empresas activas
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmpresaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodas()
    {
        var empresas = await _servicioEmpresa.ObtenerTodosAsync();
        return Ok(empresas);
    }

    // GET api/empresas/{id} - Obtiene una empresa por su identificador
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var empresa = await _servicioEmpresa.ObtenerPorIdAsync(id);
        if (empresa is null) return NotFound();
        return Ok(empresa);
    }

    // POST api/empresas - Registra una nueva empresa
    [HttpPost]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear([FromBody] CrearEmpresaDto dto)
    {
        try
        {
            var empresa = await _servicioEmpresa.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = empresa.Id }, empresa);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/empresas/{id} - Actualiza los datos de una empresa existente
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEmpresaDto dto)
    {
        var empresa = await _servicioEmpresa.ActualizarAsync(id, dto);
        if (empresa is null) return NotFound();
        return Ok(empresa);
    }

    // DELETE api/empresas/{id} - Elimina logicamente una empresa
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _servicioEmpresa.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
