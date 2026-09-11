using ReWearWeb.Models;

namespace ReWearWeb.Services.Interfaces;

public interface ICategoriaService
{
    Task<Result<IEnumerable<Categoria>>> ListarAsync();

    Task<Result<Categoria>> ObtenerPorIdAsync(int id);

    Task<Result<Categoria>> CrearAsync(Categoria categoria);

    Task<Result<Categoria>> ActualizarAsync(int id, Categoria categoria);

    Task<Result<Categoria>> CambiarEstadoAsync(int id, bool estado);
}
