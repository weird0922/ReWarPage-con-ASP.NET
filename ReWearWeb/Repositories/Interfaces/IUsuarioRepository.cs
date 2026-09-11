using System.Linq.Expressions;
using ReWearWeb.Models;

namespace ReWearWeb.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync(
        bool incluirInactivos = false,
        string? buscar = null,
        Expression<Func<Usuario, object>>? orderByDesc = null);

    Task<Usuario?> GetByIdAsync(int id);

    Task<bool> ExisteByIdAsync(int id);

    Task<bool> ExisteByEmailAsync(string email, int? excludeId = null);

    Task<Usuario> AddAsync(Usuario usuario);

    Task<Usuario> UpdateAsync(Usuario usuario);

    Task<Usuario> UpdateEstadoAsync(int id, bool estado);

    Task<int> SaveChangesAsync();
}
