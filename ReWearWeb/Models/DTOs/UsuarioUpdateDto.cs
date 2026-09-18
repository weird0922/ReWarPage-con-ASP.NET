using System.ComponentModel.DataAnnotations;
using ReWearWeb.Models.Validation;

namespace ReWearWeb.Models.DTOs;

public class UsuarioUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio. Por favor ingresa tu nombre.")]
    [MaxLength(50, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos {1} caracteres.")]
    [NombreSoloLetras]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios. Por favor ingresa tus apellidos completos.")]
    [MaxLength(80, ErrorMessage = "Los apellidos no pueden tener más de {1} caracteres.")]
    [MinLength(3, ErrorMessage = "Los apellidos deben tener al menos {1} caracteres.")]
    [NombreSoloLetras]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido. Revisa que incluya el símbolo '@' y un dominio correcto.")]
    [MaxLength(120, ErrorMessage = "El correo no puede exceder los {1} caracteres.")]
    [NoEmailsTemporales]
    public string Email { get; set; } = string.Empty;

    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos {1} caracteres cuando quieras cambiarla.")]
    [MaxLength(100, ErrorMessage = "La contraseña no puede tener más de {1} caracteres.")]
    [ContrasenaSegura]
    public string? Password { get; set; }

    [MaxLength(20, ErrorMessage = "El teléfono no puede tener más de {1} caracteres.")]
    [TelefonoPeru]
    public string? Telefono { get; set; }

    public bool Estado { get; set; } = true;
}
