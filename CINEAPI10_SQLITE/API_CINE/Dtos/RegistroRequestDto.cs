using System.ComponentModel.DataAnnotations;

namespace API_CINE.Dtos;

// REGISTRO : DTO DE ENTRADA DEL ENDPOINT POST api/auth/registro
public class RegistroRequest
{
    [Required(ErrorMessage = "EL CORREO ES OBLIGATORIO")]
    [EmailAddress(ErrorMessage = "EL CORREO NO TIENE UN FORMATO VALIDO")]
    public string Correo { get; set; } = "";

    [Required(ErrorMessage = "LA CLAVE ES OBLIGATORIA")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "LA CLAVE DEBE TENER ENTRE 6 Y 50 CARACTERES")]
    public string Clave { get; set; } = "";
}
