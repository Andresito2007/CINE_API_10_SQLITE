
namespace API_CINE.Dtos;

// RESPUESTA DEL LOGIN : EL TOKEN
// Token : EL JWT QUE EL CLIENTE ENVIARA EN CADA PETICION COMO  Authorization: Bearer <token>
public class TokenResponse(string token, DateTime expira)
{
    public string Token { get; } = token;
    public DateTime Expira { get; } = expira;
}
