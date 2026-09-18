using Microsoft.AspNetCore.Mvc;
using ReWearWeb.Models;
using ReWearWeb.Models.DTOs;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{id:int?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuarios(
        int? id,
        [FromQuery] bool incluirInactivos = false,
        [FromQuery] string? buscar = null)
    {
        if (id.HasValue)
        {
            var one = await _usuarioService.ObtenerPorIdAsync(id.Value);
            if (!one.Success)
            {
                return NotFound(new { error = one.Errors.FirstOrDefault() });
            }
            return Ok(one.Data);
        }
        else
        {
            var all = await _usuarioService.ListarAsync(incluirInactivos, buscar);
            return Ok(all.Data);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UsuarioDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "El servidor rechazó los datos porque no pasaron las validaciones requeridas (DataAnnotations + reglas personalizadas).");
            return ValidationProblem(ModelState);
        }

        var result = await _usuarioService.CrearAsync(dto);
        if (!result.Success)
        {
            return result.ErrorType == ResultErrorType.Conflict
                ? Conflict(new { error = result.Errors.FirstOrDefault() })
                : BadRequest(new { error = result.Errors.FirstOrDefault() });
        }

        return CreatedAtAction(nameof(GetUsuarios), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioDto>> PutUsuario(int id, [FromBody] UsuarioUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Revisa los mensajes de error en cada campo. El servidor valida de nuevo incluso si la validación del cliente fue omitida.");
            return ValidationProblem(ModelState);
        }

        var result = await _usuarioService.ActualizarAsync(id, dto);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ResultErrorType.NotFound => NotFound(new { error = result.Errors.FirstOrDefault() }),
                ResultErrorType.Conflict => Conflict(new { error = result.Errors.FirstOrDefault() }),
                _ => BadRequest(new { error = result.Errors.FirstOrDefault() })
            };
        }

        return Ok(result.Data);
    }

    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> PatchEstadoUsuario(int id, [FromBody] UsuarioEstadoDto dto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "El campo 'estado' es requerido y debe ser true (activo) o false (inactivo).");
            return ValidationProblem(ModelState);
        }

        var result = await _usuarioService.CambiarEstadoAsync(id, dto.Estado);
        if (!result.Success)
        {
            return NotFound(new { error = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }
}
