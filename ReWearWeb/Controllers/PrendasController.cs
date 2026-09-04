using Microsoft.AspNetCore.Mvc;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Controllers;

public class PrendasController : Controller
{
    private readonly IPrendaService _prendaService;

    public PrendasController(IPrendaService prendaService)
    {
        _prendaService = prendaService;
    }

    public async Task<IActionResult> Index()
    {
        var prendas = await _prendaService.ObtenerPrendasDestacadasAsync();
        return View(prendas);
    }
}
