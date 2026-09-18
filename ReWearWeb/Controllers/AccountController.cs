using Microsoft.AspNetCore.Mvc;
using ReWearWeb.Models.DTOs;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Controllers;

public class AccountController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public AccountController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public IActionResult Registrar()
    {
        return View(new UsuarioCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(UsuarioCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _usuarioService.CrearAsync(dto);

        if (!result.Success)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err);
            }
            return View(dto);
        }

        TempData["UsuarioCreado"] = $"Cuenta creada exitosamente. Bienvenido/a, {result.Data!.Nombre}!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}
