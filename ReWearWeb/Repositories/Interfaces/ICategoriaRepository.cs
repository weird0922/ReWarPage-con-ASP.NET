using ReWearWeb.Models;

namespace ReWearWeb.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();

    Task<Categoria?> GetByIdAsync(int id);

    Task<bool> ExisteByNombreAsync(string nombre, int? excludeId = null);

    Task<Categoria> AddAsync(Categoria categoria);

    Task<Categoria> UpdateAsync(Categoria categoria);
}
