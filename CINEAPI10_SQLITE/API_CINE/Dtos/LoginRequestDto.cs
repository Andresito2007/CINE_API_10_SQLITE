using System.ComponentModel.DataAnnotations;

namespace API_CINE.Dtos;

// LOGIN : DTO DE ENTRADA DEL ENDPOINT POST api/auth/login ( EL CLIENTE ENVIA CORREO Y CLAVE )
public class LoginRequest
{
    [Required(ErrorMessage = "EL CORREO ES OBLIGATORIO")]
    [EmailAddress(ErrorMessage = "EL CORREO NO TIENE UN FORMATO VALIDO")]
    public string Correo { get; set; } = "";

    [Required(ErrorMessage = "LA CLAVE ES OBLIGATORIA")]
    public string Clave { get; set; } = "";
}
