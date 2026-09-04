using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReWearWeb.Models;

public class Prenda
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    [Column(TypeName = "decimal(10,2)")]
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

    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    public bool EstaDisponible { get; set; } = true;

    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public virtual Usuario Vendedor { get; set; } = null!;

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey(nameof(CategoriaId))]
    public virtual Categoria Categoria { get; set; } = null!;
}
