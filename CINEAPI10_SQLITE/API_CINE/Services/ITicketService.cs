using API_CINE.Dtos;
using API_CINE.Models;
namespace API_CINE.Services;

// NUESTRO CONTRATO ( PARA EL CONTROLADOR) : CUALQUIER CLASE QUE IMPLEMENTE ESTA INTERFAZ DEBE TENER ESTOS METODOS
public interface ITicketService
{
    List<Ticket> ObtenerTodos();
    Ticket ObtenerPorId(int id);

    List<Ticket> ListarTicketsDeCliente(int clienteId);
    Ticket Crear(TicketRequestDto dto);
    Ticket Actualizar(int id, TicketRequestDto dto);
    void Eliminar(int id);
}
