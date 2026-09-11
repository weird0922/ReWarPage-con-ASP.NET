using ReWearWeb.Models;

namespace ReWearWeb.Repositories.Interfaces;

public interface IPrendaRepository
{
    Task<List<Prenda>> GetDestacadasAsync(int cantidad);

    Task<Prenda?> GetByIdAsync(int id);

    Task<Prenda> AddAsync(Prenda prenda);

    Task<Prenda> UpdateAsync(Prenda prenda);

    Task<List<Prenda>> GetByCategoriaIdAsync(int categoriaId);
}
