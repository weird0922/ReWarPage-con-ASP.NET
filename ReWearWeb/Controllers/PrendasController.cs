using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReWearWeb.Models;
using ReWearWeb.Models.DTOs;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Controllers;

public class PrendasController : Controller
{
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long TamanoMaximoBytes = 5 * 1024 * 1024;

    private readonly IPrendaService _prendaService;
    private readonly ICategoriaService _categoriaService;
    private readonly IUsuarioService _usuarioService;
    private readonly IWebHostEnvironment _environment;

    public PrendasController(
        IPrendaService prendaService,
        ICategoriaService categoriaService,
        IUsuarioService usuarioService,
        IWebHostEnvironment environment)
    {
        _prendaService = prendaService;
        _categoriaService = categoriaService;
        _usuarioService = usuarioService;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var prendas = await _prendaService.ObtenerPrendasDestacadasAsync();
        return View(prendas);
    }

    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View(new PrendaFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrendaFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(vm.CategoriaId, vm.UsuarioId);
            return View(vm);
        }

        string? imagenUrl = null;
        if (vm.Imagen != null)
        {
            var (url, error) = await GuardarImagenAsync(vm.Imagen);
            if (error != null)
            {
                ModelState.AddModelError(nameof(vm.Imagen), error);
                await CargarListasAsync(vm.CategoriaId, vm.UsuarioId);
                return View(vm);
            }
            imagenUrl = url;
        }

        var result = await _prendaService.CrearAsync(MapearDesdeFormulario(vm, imagenUrl));
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "No se pudo publicar la prenda.");
            await CargarListasAsync(vm.CategoriaId, vm.UsuarioId);
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _prendaService.ObtenerPorIdAsync(id.Value);
        if (!result.Success)
        {
            return NotFound();
        }

        var prenda = result.Data!;
        var vm = new PrendaFormViewModel
        {
            Id = prenda.Id,
            Titulo = prenda.Titulo,
            Descripcion = prenda.Descripcion,
            Precio = prenda.Precio,
            Talla = prenda.Talla,
            Estado = prenda.Estado,
            Marca = prenda.Marca,
            Color = prenda.Color,
            UsuarioId = prenda.UsuarioId,
            CategoriaId = prenda.CategoriaId,
            ImagenUrlActual = prenda.ImagenUrl,
            VendedorNombreCompleto = $"{prenda.Vendedor.Nombre} {prenda.Vendedor.Apellidos}"
        };

        await CargarListasAsync(vm.CategoriaId);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrendaFormViewModel vm)
    {
        if (id != vm.Id)
        {
            return NotFound();
        }

        var actual = await _prendaService.ObtenerPorIdAsync(id);
        if (!actual.Success)
        {
            return NotFound();
        }

        // El vendedor es quien publicó la prenda y no puede reasignarse desde Edit,
        // así que se ignora cualquier UsuarioId recibido del formulario.
        vm.UsuarioId = actual.Data!.UsuarioId;
        ModelState.Remove(nameof(PrendaFormViewModel.UsuarioId));
        vm.VendedorNombreCompleto = $"{actual.Data!.Vendedor.Nombre} {actual.Data!.Vendedor.Apellidos}";

        if (!ModelState.IsValid)
        {
            vm.ImagenUrlActual = actual.Data!.ImagenUrl;
            await CargarListasAsync(vm.CategoriaId);
            return View(vm);
        }

        var imagenUrl = actual.Data!.ImagenUrl;
        if (vm.Imagen != null)
        {
            var (url, error) = await GuardarImagenAsync(vm.Imagen);
            if (error != null)
            {
                ModelState.AddModelError(nameof(vm.Imagen), error);
                vm.ImagenUrlActual = imagenUrl;
                await CargarListasAsync(vm.CategoriaId);
                return View(vm);
            }
            EliminarImagenFisica(imagenUrl);
            imagenUrl = url;
        }

        var result = await _prendaService.ActualizarAsync(id, MapearDesdeFormulario(vm, imagenUrl));
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "No se pudo actualizar la prenda.");
            vm.ImagenUrlActual = imagenUrl;
            await CargarListasAsync(vm.CategoriaId);
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int? categoriaSeleccionada = null, int? usuarioSeleccionado = null)
    {
        var categorias = await _categoriaService.ListarAsync();
        var usuarios = await _usuarioService.ListarAsync(incluirInactivos: false);

        ViewBag.Categorias = new SelectList(
            categorias.Data!.Where(c => c.Estado), "Id", "Nombre", categoriaSeleccionada);

        ViewBag.Usuarios = new SelectList(
            usuarios.Data!.Select(u => new { u.Id, NombreCompleto = $"{u.Nombre} {u.Apellidos}" }),
            "Id", "NombreCompleto", usuarioSeleccionado);
    }

    private static Prenda MapearDesdeFormulario(PrendaFormViewModel vm, string? imagenUrl)
    {
        return new Prenda
        {
            Titulo = vm.Titulo.Trim(),
            Descripcion = vm.Descripcion.Trim(),
            Precio = vm.Precio,
            Talla = vm.Talla.Trim(),
            Estado = vm.Estado.Trim(),
            Marca = string.IsNullOrWhiteSpace(vm.Marca) ? null : vm.Marca.Trim(),
            Color = string.IsNullOrWhiteSpace(vm.Color) ? null : vm.Color.Trim(),
            UsuarioId = vm.UsuarioId,
            CategoriaId = vm.CategoriaId,
            ImagenUrl = imagenUrl
        };
    }

    private async Task<(string? Url, string? Error)> GuardarImagenAsync(IFormFile imagen)
    {
        var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            return (null, "Solo se permiten imágenes JPG, PNG o WEBP.");
        }

        if (imagen.Length > TamanoMaximoBytes)
        {
            return (null, "La imagen no debe superar los 5 MB.");
        }

        var carpeta = Path.Combine(_environment.WebRootPath, "uploads", "prendas");
        Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await imagen.CopyToAsync(stream);
        }

        return ($"/uploads/prendas/{nombreArchivo}", null);
    }

    private void EliminarImagenFisica(string? imagenUrl)
    {
        if (string.IsNullOrEmpty(imagenUrl))
        {
            return;
        }

        var rutaFisica = Path.Combine(_environment.WebRootPath, imagenUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(rutaFisica))
        {
            System.IO.File.Delete(rutaFisica);
        }
    }
}
