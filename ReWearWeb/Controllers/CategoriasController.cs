using Microsoft.AspNetCore.Mvc;
using ReWearWeb.Models;
using ReWearWeb.Models.DTOs;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Controllers;

public class CategoriasController : Controller
{
    private readonly ICategoriaService _categoriaService;
    private readonly IPrendaService _prendaService;

    public CategoriasController(ICategoriaService categoriaService, IPrendaService prendaService)
    {
        _categoriaService = categoriaService;
        _prendaService = prendaService;
    }

    // GET: Categorias
    public async Task<IActionResult> Index()
    {
        var result = await _categoriaService.ListarAsync();
        return View(result.Data);
    }

    // GET: Categorias/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _categoriaService.ObtenerPorIdAsync(id.Value);
        if (!result.Success)
        {
            return NotFound();
        }

        var vm = new CategoriaDetailsViewModel
        {
            Categoria = result.Data!,
            Prendas = await _prendaService.ObtenerPorCategoriaAsync(id.Value)
        };

        return View(vm);
    }

    // GET: Categorias/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categorias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Estado")] Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        var result = await _categoriaService.CrearAsync(categoria);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "No se pudo crear la categoría.");
            return View(categoria);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Categorias/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _categoriaService.ObtenerPorIdAsync(id.Value);
        if (!result.Success)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    // POST: Categorias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Categoria categoria)
    {
        if (id != categoria.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(categoria);
        }

        var result = await _categoriaService.ActualizarAsync(id, categoria);
        if (!result.Success)
        {
            if (result.ErrorType == ResultErrorType.NotFound)
            {
                return NotFound();
            }
            ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "No se pudo actualizar la categoría.");
            return View(categoria);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Categorias/ToggleEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleEstado(int id)
    {
        var actual = await _categoriaService.ObtenerPorIdAsync(id);
        if (!actual.Success)
        {
            return NotFound();
        }

        var result = await _categoriaService.CambiarEstadoAsync(id, !actual.Data!.Estado);
        if (!result.Success)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Categorias/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _categoriaService.ObtenerPorIdAsync(id.Value);
        if (!result.Success)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    // POST: Categorias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _categoriaService.CambiarEstadoAsync(id, false);
        if (!result.Success)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
