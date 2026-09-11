using ReWearWeb.Models;
using ReWearWeb.Repositories.Interfaces;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<Result<IEnumerable<Categoria>>> ListarAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return Result<IEnumerable<Categoria>>.Ok(categorias);
    }

    public async Task<Result<Categoria>> ObtenerPorIdAsync(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
        {
            return Result<Categoria>.Fail($"Categoría con id {id} no encontrada.", ResultErrorType.NotFound);
        }
        return Result<Categoria>.Ok(categoria);
    }

    public async Task<Result<Categoria>> CrearAsync(Categoria categoria)
    {
        var nombreNormalizado = categoria.Nombre.Trim();

        if (await _categoriaRepository.ExisteByNombreAsync(nombreNormalizado))
        {
            return Result<Categoria>.Fail($"Ya existe una categoría con el nombre {nombreNormalizado}.", ResultErrorType.Conflict);
        }

        categoria.Nombre = nombreNormalizado;
        categoria.Descripcion = string.IsNullOrWhiteSpace(categoria.Descripcion) ? null : categoria.Descripcion.Trim();

        var creada = await _categoriaRepository.AddAsync(categoria);
        return Result<Categoria>.Ok(creada);
    }

    public async Task<Result<Categoria>> ActualizarAsync(int id, Categoria categoria)
    {
        var existente = await _categoriaRepository.GetByIdAsync(id);
        if (existente == null)
        {
            return Result<Categoria>.Fail($"Categoría con id {id} no encontrada.", ResultErrorType.NotFound);
        }

        var nombreNormalizado = categoria.Nombre.Trim();
        if (await _categoriaRepository.ExisteByNombreAsync(nombreNormalizado, excludeId: id))
        {
            return Result<Categoria>.Fail($"Ya existe otra categoría con el nombre {nombreNormalizado}.", ResultErrorType.Conflict);
        }

        existente.Nombre = nombreNormalizado;
        existente.Descripcion = string.IsNullOrWhiteSpace(categoria.Descripcion) ? null : categoria.Descripcion.Trim();
        // El Estado se gestiona únicamente desde ToggleEstado (el switch del Index), no desde este formulario.

        var actualizada = await _categoriaRepository.UpdateAsync(existente);
        return Result<Categoria>.Ok(actualizada);
    }

    public async Task<Result<Categoria>> CambiarEstadoAsync(int id, bool estado)
    {
        var existente = await _categoriaRepository.GetByIdAsync(id);
        if (existente == null)
        {
            return Result<Categoria>.Fail($"Categoría con id {id} no encontrada.", ResultErrorType.NotFound);
        }

        existente.Estado = estado;
        var actualizada = await _categoriaRepository.UpdateAsync(existente);
        return Result<Categoria>.Ok(actualizada);
    }
}
