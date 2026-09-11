using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReWearWeb.Models;

public class Compra
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaCompra { get; set; } = DateTime.Now;

    [Required]
    public int UsuarioCompradorId { get; set; }

    [ForeignKey(nameof(UsuarioCompradorId))]
    public virtual Usuario Comprador { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Impuesto { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [MaxLength(30)]
    public string MetodoPago { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Estado { get; set; } = "Pendiente";

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();
}
