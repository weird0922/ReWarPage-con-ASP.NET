using ReWearWeb.Models;
using ReWearWeb.Models.DTOs;

namespace ReWearWeb.Services.Interfaces;

public interface IUsuarioService
{
    Task<Result<IEnumerable<UsuarioDto>>> ListarAsync(bool incluirInactivos = false, string? buscar = null);

    Task<Result<UsuarioDto>> ObtenerPorIdAsync(int id);

    Task<Result<UsuarioDto>> CrearAsync(UsuarioCreateDto dto);

    Task<Result<UsuarioDto>> ActualizarAsync(int id, UsuarioUpdateDto dto);

    Task<Result<UsuarioDto>> CambiarEstadoAsync(int id, bool estado);

    Task<Result<UsuarioDto>> ToggleEstadoAsync(int id);
}
