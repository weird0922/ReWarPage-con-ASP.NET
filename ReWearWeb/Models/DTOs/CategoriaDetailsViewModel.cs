namespace ReWearWeb.Models.DTOs;

public class CategoriaDetailsViewModel
{
    public Categoria Categoria { get; set; } = null!;

    public List<Prenda> Prendas { get; set; } = new();
}
