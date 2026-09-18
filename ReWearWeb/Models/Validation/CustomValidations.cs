using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReWearWeb.Models.Validation;

public class NoEmailsTemporalesAttribute : ValidationAttribute
{
    private static readonly string[] DominiosBloqueados = new[]
    {
        "yopmail.com", "yopmail.es", "yopmail.net",
        "tempmail.com", "temp-mail.org", "10minutemail.com",
        "10minutemail.net", "throwawaymail.com", "dispostable.com",
        "guerrillamail.com", "guerrillamail.net", "guerrillamailblock.com",
        "sharklasers.com", "guerrillamail.org", "mailinator.com",
        "mailinator.net", "mailinator2.com", "sogetthis.com",
        "fakeinbox.com", "fakeinbox.org", "fakemail.net",
        "temporary-mail.net", "mytemp.email", "tempail.com",
        "spam4.me", "dayrep.com", "rhyta.com",
        "einrot.com", "fleckens.hu", "superrito.com",
        "teleworm.us", "superstachel.de", "armyspy.com",
        "cuvox.de", "gustr.com", "jourrapide.com"
    };

    public NoEmailsTemporalesAttribute()
    {
        ErrorMessage = "El dominio de correo '{0}' no es válido en reWear. Por tu seguridad, no aceptamos correos temporales o desechables. Usa un correo real (Gmail, Outlook, institucional, etc).";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string email || string.IsNullOrWhiteSpace(email))
        {
            return true;
        }

        if (!email.Contains('@'))
        {
            return true;
        }

        var dominio = email.Split('@', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).LastOrDefault() ?? string.Empty;
        dominio = dominio.ToLowerInvariant();

        foreach (var domBloq in DominiosBloqueados)
        {
            if (dominio.Equals(domBloq, StringComparison.OrdinalIgnoreCase) || dominio.EndsWith("." + domBloq, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name);
    }
}

public class ContrasenaSeguraAttribute : ValidationAttribute
{
    private static readonly string[] ContrasenasComunes = new[]
    {
        "123456", "12345678", "123456789", "12345",
        "password", "contraseña", "contrasena",
        "qwerty", "qwerty123", "abc123", "1234567",
        "111111", "admin", "admin123",
        "iloveyou", "1q2w3e4r", "000000",
        "654321", "letmein", "welcome",
        "monkey", "dragon", "master",
        "rewear", "rewear123", "usuario", "usuario123",
        "clave", "clave123", "asdfgh", "asdf"
    };

    public int MinimoDiversidadCaracteres { get; set; } = 2;

    public ContrasenaSeguraAttribute()
    {
        ErrorMessage = "La contraseña no es segura para reWear: evita contraseñas comunes como '123456', 'admin' o 'qwerty'. Incluye al menos 2 tipos de caracteres (letras y números, por ejemplo).";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string password || string.IsNullOrWhiteSpace(password))
        {
            return true;
        }

        var pass = password.Trim().ToLowerInvariant();

        if (ContrasenasComunes.Contains(pass))
        {
            return false;
        }

        int tipos = 0;
        if (Regex.IsMatch(password, @"[a-záéíóúñü]")) tipos++;
        if (Regex.IsMatch(password, @"[A-ZÁÉÍÓÚÑÜ]")) tipos++;
        if (Regex.IsMatch(password, @"[0-9]")) tipos++;
        if (Regex.IsMatch(password, @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ]")) tipos++;

        return tipos >= MinimoDiversidadCaracteres;
    }
}

public class TelefonoPeruAttribute : ValidationAttribute
{
    public TelefonoPeruAttribute()
    {
        ErrorMessage = "El teléfono ingresado no tiene un formato válido para Perú. Debe empezar con 9 y contener 9 dígitos numéricos (ej: 987654321).";
    }

    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        if (value is not string telefono || string.IsNullOrWhiteSpace(telefono))
        {
            return true;
        }

        var soloNumeros = new string(telefono.Where(char.IsDigit).ToArray());
        if (soloNumeros.Length == 0) return true;
        if (soloNumeros.Length != 9) return false;
        if (!soloNumeros.StartsWith('9')) return false;

        return true;
    }
}

public class NombreSoloLetrasAttribute : ValidationAttribute
{
    public NombreSoloLetrasAttribute()
    {
        ErrorMessage = "El campo '{0}' solo puede contener letras, espacios y tildes. No se permiten números ni símbolos.";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string texto || string.IsNullOrWhiteSpace(texto))
        {
            return true;
        }

        return Regex.IsMatch(texto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'\-]+$");
    }
}
