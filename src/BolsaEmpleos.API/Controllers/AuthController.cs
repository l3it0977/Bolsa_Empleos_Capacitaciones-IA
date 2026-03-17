using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BolsaEmpleos.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IRepositorioEmpresa _repositorioEmpresa;
    private readonly IConfiguration _configuracion;

    public AuthController(
        IRepositorioJoven repositorioJoven,
        IRepositorioEmpresa repositorioEmpresa,
        IConfiguration configuracion)
    {
        _repositorioJoven = repositorioJoven;
        _repositorioEmpresa = repositorioEmpresa;
        _configuracion = configuracion;
    }

    [HttpPost("login-joven")]
    [ProducesResponseType(typeof(RespuestaAutenticacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginJoven([FromBody] LoginDto dto)
    {
        var joven = await _repositorioJoven.ObtenerPorCorreoAsync(dto.CorreoElectronico);
        if (joven is null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, joven.ContrasenaHash))
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        return Ok(CrearRespuesta(joven.Id, "Joven", joven.CorreoElectronico, joven.Nombre, joven.Apellido));
    }

    [HttpPost("login-empresa")]
    [ProducesResponseType(typeof(RespuestaAutenticacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginEmpresa([FromBody] LoginDto dto)
    {
        var empresa = await _repositorioEmpresa.ObtenerPorCorreoAsync(dto.CorreoElectronico);
        if (empresa is null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, empresa.ContrasenaHash))
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        return Ok(CrearRespuesta(empresa.Id, "Empresa", empresa.CorreoElectronico, empresa.RazonSocial));
    }

    private RespuestaAutenticacionDto CrearRespuesta(
        int idUsuario,
        string rol,
        string correoElectronico,
        string nombrePrincipal,
        string? nombreSecundario = null)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
            new Claim(ClaimTypes.Role, rol),
            new Claim(ClaimTypes.Email, correoElectronico)
        };

        var clave = _configuracion["Jwt:ClaveSecreta"] ?? throw new InvalidOperationException("Falta configurar Jwt:ClaveSecreta.");
        var expiracionMinutos = _configuracion.GetValue<int>("Jwt:ExpiracionMinutos", 120);

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiracionMinutos),
            signingCredentials: credenciales);

        return new RespuestaAutenticacionDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEn = token.ValidTo,
            Usuario = new UsuarioAutenticadoDto
            {
                Id = idUsuario,
                Rol = rol,
                CorreoElectronico = correoElectronico,
                Nombre = nombrePrincipal,
                NombreSecundario = nombreSecundario
            }
        };
    }
}

public class LoginDto
{
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}

public class RespuestaAutenticacionDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
    public UsuarioAutenticadoDto Usuario { get; set; } = new();
}

public class UsuarioAutenticadoDto
{
    public int Id { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? NombreSecundario { get; set; }
}
