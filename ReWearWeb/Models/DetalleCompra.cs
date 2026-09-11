using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReWearWeb.Models;

public class DetalleCompra
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompraId { get; set; }

    [ForeignKey(nameof(CompraId))]
    public virtual Compra Compra { get; set; } = null!;

    [Required]
    public int PrendaId { get; set; }

    [ForeignKey(nameof(PrendaId))]
    public virtual Prenda Prenda { get; set; } = null!;

    [Required]
    public int Cantidad { get; set; } = 1;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioUnitario { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }
}
