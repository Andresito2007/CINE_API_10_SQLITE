
namespace API_CINE.Models;

using System.Text.Json.Serialization;

// ENTIDAD CLIENTE

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Dni { get; set; } = "";
    public string Email { get; set; } = "";

    public string? Telefono { get; set; }

    // SE OMITE EN EL JSON PARA EVITAR UN CICLO ( Cliente -> Tickets -> Cliente .. Bucle )
    [JsonIgnore]
    public List<Ticket> Tickets { get; private set; } = [];

    public Cliente() { }

    public Cliente(int id, string nombre, string dni, string email)
    {
        Id = id;
        Nombre = nombre;
        Dni = dni;
        Email = email;
    }
}
