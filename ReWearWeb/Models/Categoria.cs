using System.ComponentModel.DataAnnotations;

namespace ReWearWeb.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    public virtual ICollection<Prenda> Prendas { get; set; } = new List<Prenda>();
}
