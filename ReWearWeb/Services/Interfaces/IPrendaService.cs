using ReWearWeb.Models;

namespace ReWearWeb.Services.Interfaces;

public interface IPrendaService
{
    Task<List<Prenda>> ObtenerPrendasDestacadasAsync(int cantidad = 5);

    Task<Result<Prenda>> ObtenerPorIdAsync(int id);

    Task<Result<Prenda>> CrearAsync(Prenda prenda);

    Task<Result<Prenda>> ActualizarAsync(int id, Prenda prenda);

    Task<List<Prenda>> ObtenerPorCategoriaAsync(int categoriaId);
}
