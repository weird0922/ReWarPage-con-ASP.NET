using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ReWearWeb.Models.DTOs;

public class PrendaFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(100)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(1000)]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "La talla es obligatoria")]
    [MaxLength(10)]
    public string Talla { get; set; } = string.Empty;

    [Required(ErrorMessage = "El estado es obligatorio")]
    [MaxLength(30)]
    public string Estado { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Marca { get; set; }

    [MaxLength(30)]
    public string? Color { get; set; }

    [Required(ErrorMessage = "Selecciona un vendedor")]
    [Display(Name = "Vendedor")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "Selecciona una categoría")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    [Display(Name = "Foto de la prenda")]
    public IFormFile? Imagen { get; set; }

    public string? ImagenUrlActual { get; set; }

    public string? VendedorNombreCompleto { get; set; }
}
