using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReWearWeb.Models;

public class DetalleVenta
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int VentaId { get; set; }

    [ForeignKey(nameof(VentaId))]
    public virtual Venta Venta { get; set; } = null!;

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
