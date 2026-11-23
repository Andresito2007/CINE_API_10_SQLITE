namespace API_CINE.Dtos;

// RESPUESTA DEL REGISTRO Y DEL ENDPOINT "YO"
public class UsuarioResponse(string id, string correo)
{
    public string Id { get; } = id;
    public string Correo { get; } = correo;
}
