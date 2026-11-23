namespace API_CINE.Models;

using System.Text.Json.Serialization;

// ENTIDAD TICKET
public class Ticket
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int PeliculaId { get; set; }
    public DateTime FechaFuncion { get; set; }
    public string Sala { get; set; } = "";
    public string Asiento { get; set; } = "";
    public decimal Precio { get; set; }
    public string Estado { get; set; } = "Activo";

    // PROPIEDAD DE NAVEGACION : NO ES UNA COLUMNA , ES UN ATAJO PARA LLEGAR AL CLIENTE
    [JsonIgnore]
    public Cliente? Cliente { get; private set; }
    public Ticket() { }

    public Ticket(int id, int clienteId, int peliculaId, DateTime fechaFuncion,
                  string sala, string asiento, decimal precio)
    {
        Id = id;
        ClienteId = clienteId;
        PeliculaId = peliculaId;
        FechaFuncion = fechaFuncion;
        Sala = sala;
        Asiento = asiento;
        Precio = precio;
    }
}
