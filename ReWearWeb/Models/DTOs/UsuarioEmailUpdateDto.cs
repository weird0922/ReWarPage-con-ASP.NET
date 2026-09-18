using System.ComponentModel.DataAnnotations;
using ReWearWeb.Models.Validation;

namespace ReWearWeb.Models.DTOs;

public class UsuarioEmailUpdateDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido. Asegúrate de incluir el símbolo '@'.")]
    [MaxLength(120, ErrorMessage = "El correo no puede exceder los {1} caracteres.")]
    [NoEmailsTemporales]
    public string Email { get; set; } = string.Empty;
}
