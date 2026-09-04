using ReWearWeb.Models;

namespace ReWearWeb.Services.Interfaces;

public interface IPrendaService
{
    Task<List<Prenda>> ObtenerPrendasDestacadasAsync(int cantidad = 5);
}
