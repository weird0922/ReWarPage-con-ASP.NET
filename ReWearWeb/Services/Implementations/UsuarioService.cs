using ReWearWeb.Models;
using ReWearWeb.Models.DTOs;
using ReWearWeb.Repositories.Interfaces;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Services.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<IEnumerable<UsuarioDto>>> ListarAsync(bool incluirInactivos = false, string? buscar = null)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(
            incluirInactivos,
            buscar,
            u => u.FechaRegistro);

        var dtos = usuarios.Select(MapToDto).ToList();
        return Result<IEnumerable<UsuarioDto>>.Ok(dtos);
    }

    public async Task<Result<UsuarioDto>> ObtenerPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<UsuarioDto>.Fail($"Usuario con id {id} no encontrado.", ResultErrorType.NotFound);
        }
        return Result<UsuarioDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result<UsuarioDto>> CrearAsync(UsuarioCreateDto dto)
    {
        var emailNormalizado = dto.Email.Trim();

        if (await _usuarioRepository.ExisteByEmailAsync(emailNormalizado))
        {
            return Result<UsuarioDto>.Fail($"Ya existe un usuario con el email {emailNormalizado}.", ResultErrorType.Conflict);
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            Email = emailNormalizado,
            Password = dto.Password,
            Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim(),
            FechaRegistro = DateTime.Now,
            Estado = true
        };

        var creado = await _usuarioRepository.AddAsync(usuario);
        return Result<UsuarioDto>.Ok(MapToDto(creado));
    }

    public async Task<Result<UsuarioDto>> ActualizarEmailAsync(int id, UsuarioEmailUpdateDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<UsuarioDto>.Fail($"Usuario con id {id} no encontrado.", ResultErrorType.NotFound);
        }

        var emailNormalizado = dto.Email.Trim();
        if (await _usuarioRepository.ExisteByEmailAsync(emailNormalizado, excludeId: id))
        {
            return Result<UsuarioDto>.Fail($"Ya existe otro usuario con el email {emailNormalizado}.", ResultErrorType.Conflict);
        }

        usuario.Email = emailNormalizado;

        var actualizado = await _usuarioRepository.UpdateAsync(usuario);
        return Result<UsuarioDto>.Ok(MapToDto(actualizado));
    }

    public async Task<Result<UsuarioDto>> CambiarEstadoAsync(int id, bool estado)
    {
        if (!await _usuarioRepository.ExisteByIdAsync(id))
        {
            return Result<UsuarioDto>.Fail($"Usuario con id {id} no encontrado.", ResultErrorType.NotFound);
        }

        var usuario = await _usuarioRepository.UpdateEstadoAsync(id, estado);
        return Result<UsuarioDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result<UsuarioDto>> ToggleEstadoAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<UsuarioDto>.Fail($"Usuario con id {id} no encontrado.", ResultErrorType.NotFound);
        }

        usuario.Estado = !usuario.Estado;
        var actualizado = await _usuarioRepository.UpdateAsync(usuario);
        return Result<UsuarioDto>.Ok(MapToDto(actualizado));
    }

    private static UsuarioDto MapToDto(Usuario u)
    {
        return new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Apellidos = u.Apellidos,
            Email = u.Email,
            Telefono = u.Telefono,
            FechaRegistro = u.FechaRegistro,
            Estado = u.Estado
        };
    }
}
