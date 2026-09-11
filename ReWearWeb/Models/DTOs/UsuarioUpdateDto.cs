using System.ComponentModel.DataAnnotations;

namespace ReWearWeb.Models.DTOs;

public class UsuarioUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [MaxLength(80)]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email no válido")]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Password { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    public bool Estado { get; set; } = true;
}
