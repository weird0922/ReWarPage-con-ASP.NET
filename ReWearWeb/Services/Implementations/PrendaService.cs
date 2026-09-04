using Microsoft.EntityFrameworkCore;
using ReWearWeb.Data;
using ReWearWeb.Models;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Services.Implementations;

public class PrendaService : IPrendaService
{
    private readonly AppDbContext _context;

    public PrendaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Prenda>> ObtenerPrendasDestacadasAsync(int cantidad = 5)
    {
        return await _context.Prendas
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .Where(p => p.EstaDisponible)
            .OrderByDescending(p => p.FechaPublicacion)
            .Take(cantidad)
            .ToListAsync();
    }
}
