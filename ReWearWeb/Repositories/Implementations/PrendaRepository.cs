using Microsoft.EntityFrameworkCore;
using ReWearWeb.Data;
using ReWearWeb.Models;
using ReWearWeb.Repositories.Interfaces;

namespace ReWearWeb.Repositories.Implementations;

public class PrendaRepository : IPrendaRepository
{
    private readonly AppDbContext _context;

    public PrendaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Prenda>> GetDestacadasAsync(int cantidad)
    {
        return await _context.Prendas
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .Where(p => p.EstaDisponible)
            .OrderByDescending(p => p.FechaPublicacion)
            .Take(cantidad)
            .ToListAsync();
    }

    public async Task<Prenda?> GetByIdAsync(int id)
    {
        return await _context.Prendas
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Prenda> AddAsync(Prenda prenda)
    {
        _context.Prendas.Add(prenda);
        await _context.SaveChangesAsync();
        return prenda;
    }

    public async Task<Prenda> UpdateAsync(Prenda prenda)
    {
        _context.Entry(prenda).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return prenda;
    }

    public async Task<List<Prenda>> GetByCategoriaIdAsync(int categoriaId)
    {
        return await _context.Prendas
            .Include(p => p.Vendedor)
            .Where(p => p.CategoriaId == categoriaId)
            .OrderByDescending(p => p.FechaPublicacion)
            .ToListAsync();
    }
}
