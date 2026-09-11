using System.ComponentModel.DataAnnotations;

namespace ReWearWeb.Models.DTOs;

public class UsuarioCreateDto
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

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }
}
