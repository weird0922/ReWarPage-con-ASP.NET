using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReWearWeb.Data;

namespace ReWearWeb.Controllers;

public class PrendasController : Controller
{
    private readonly AppDbContext _context;

    public PrendasController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var prendas = await _context.Prendas
                              .Include(p => p.Categoria)
                              .Include(p => p.Vendedor)
                              .Where(p => p.EstaDisponible)
                              .OrderByDescending(p => p.FechaPublicacion)
                              .Take(5)
                              .ToListAsync();

        return View(prendas);
    }
}
