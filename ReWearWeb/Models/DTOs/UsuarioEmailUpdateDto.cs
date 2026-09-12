using System.ComponentModel.DataAnnotations;

namespace ReWearWeb.Models.DTOs;

public class UsuarioEmailUpdateDto
{
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email no válido")]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;
}
