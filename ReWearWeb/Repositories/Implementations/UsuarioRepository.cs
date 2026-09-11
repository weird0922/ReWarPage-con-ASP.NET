using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReWearWeb.Data;
using ReWearWeb.Models;
using ReWearWeb.Repositories.Interfaces;

namespace ReWearWeb.Repositories.Implementations;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(
        bool incluirInactivos = false,
        string? buscar = null,
        Expression<Func<Usuario, object>>? orderByDesc = null)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!incluirInactivos)
        {
            query = query.Where(u => u.Estado);
        }

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim().ToLower();
            query = query.Where(u =>
                u.Nombre.ToLower().Contains(buscar) ||
                u.Apellidos.ToLower().Contains(buscar) ||
                u.Email.ToLower().Contains(buscar));
        }

        if (orderByDesc != null)
        {
            query = query.OrderByDescending(orderByDesc);
        }

        return await query.ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<bool> ExisteByIdAsync(int id)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExisteByEmailAsync(string email, int? excludeId = null)
    {
        var query = _context.Usuarios.Where(u => u.Email == email);
        if (excludeId.HasValue)
        {
            query = query.Where(u => u.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        _context.Entry(usuario).State = EntityState.Modified;
        await SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> UpdateEstadoAsync(int id, bool estado)
    {
        var usuario = await GetByIdAsync(id) ?? throw new InvalidOperationException($"Usuario {id} no existe.");
        usuario.Estado = estado;
        _context.Entry(usuario).State = EntityState.Modified;
        await SaveChangesAsync();
        return usuario;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
