using ReWearWeb.Models;
using ReWearWeb.Repositories.Interfaces;
using ReWearWeb.Services.Interfaces;

namespace ReWearWeb.Services.Implementations;

public class PrendaService : IPrendaService
{
    private readonly IPrendaRepository _prendaRepository;

    public PrendaService(IPrendaRepository prendaRepository)
    {
        _prendaRepository = prendaRepository;
    }

    public async Task<List<Prenda>> ObtenerPrendasDestacadasAsync(int cantidad = 5)
    {
        return await _prendaRepository.GetDestacadasAsync(cantidad);
    }

    public async Task<Result<Prenda>> ObtenerPorIdAsync(int id)
    {
        var prenda = await _prendaRepository.GetByIdAsync(id);
        if (prenda == null)
        {
            return Result<Prenda>.Fail($"Prenda con id {id} no encontrada.", ResultErrorType.NotFound);
        }
        return Result<Prenda>.Ok(prenda);
    }

    public async Task<Result<Prenda>> CrearAsync(Prenda prenda)
    {
        var creada = await _prendaRepository.AddAsync(prenda);
        return Result<Prenda>.Ok(creada);
    }

    public async Task<Result<Prenda>> ActualizarAsync(int id, Prenda prenda)
    {
        var existente = await _prendaRepository.GetByIdAsync(id);
        if (existente == null)
        {
            return Result<Prenda>.Fail($"Prenda con id {id} no encontrada.", ResultErrorType.NotFound);
        }

        existente.Titulo = prenda.Titulo;
        existente.Descripcion = prenda.Descripcion;
        existente.Precio = prenda.Precio;
        existente.Talla = prenda.Talla;
        existente.Estado = prenda.Estado;
        existente.Marca = prenda.Marca;
        existente.Color = prenda.Color;
        existente.UsuarioId = prenda.UsuarioId;
        existente.CategoriaId = prenda.CategoriaId;
        existente.ImagenUrl = prenda.ImagenUrl;

        var actualizada = await _prendaRepository.UpdateAsync(existente);
        return Result<Prenda>.Ok(actualizada);
    }

    public async Task<List<Prenda>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        return await _prendaRepository.GetByCategoriaIdAsync(categoriaId);
    }
}
