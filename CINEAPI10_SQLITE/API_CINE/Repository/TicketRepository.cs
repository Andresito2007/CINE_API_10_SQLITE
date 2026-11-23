using API_CINE.Data;
using API_CINE.Models;

namespace API_CINE.Repository;

// REPOSITORIO PARA LAS CONSULTAS A LA BD

public class TicketRepository(AppDbContext _contexto) : ITicketRepository
{

    // USANDON METODOS LINQ
    public List<Ticket> ObtenerTodos()
    {
        return _contexto.Tickets.ToList();
    }

    public Ticket? ObtenerPorId(int id)
    {
        return _contexto.Tickets.FirstOrDefault(t => t.Id == id);
    }

    public List<Ticket> ObtenerPorCliente(int clienteId)
    {
       return _contexto.Tickets.Where(t => t.ClienteId == clienteId).ToList();
    }

    // USANDO METODOS DE EF CORE
    public void Agregar(Ticket ticket)
    {
        _contexto.Tickets.Add(ticket);
        _contexto.SaveChanges(); // guardar los cambios en la base de datos
    }

    public void Actualizar(Ticket ticket)
    {
        _contexto.Tickets.Update(ticket);
        _contexto.SaveChanges();
    }

    public void Eliminar(Ticket ticket)
    {
        _contexto.Tickets.Remove(ticket);
        _contexto.SaveChanges();
    }
}
