using API_CINE.Dtos;
using API_CINE.Exceptions;
using API_CINE.Models;
using API_CINE.Repository;

namespace API_CINE.Services;

// NUESTRA LOGICA DE NEGOCIO

public class TicketService : ITicketService
{
    // (INYECCION DE DEPENDENCIAS)
    private ITicketRepository _ticketRepository;

    private IClienteRepository _clienteRepository;

    private IPeliculaRepository _peliculaRepository;

    public TicketService(ITicketRepository ticketRepository,IClienteRepository clienteRepository,IPeliculaRepository peliculaRepository)

    {
      _ticketRepository = ticketRepository;
      _clienteRepository = clienteRepository;
      _peliculaRepository = peliculaRepository;
    }

    public List<Ticket> ObtenerTodos()
    {
        return _ticketRepository.ObtenerTodos();
    }

    public Ticket ObtenerPorId(int id)
    {
        Ticket? ticket = _ticketRepository.ObtenerPorId(id); // PUEDE SER NULL ?

        if (ticket == null)
        {
            throw new NoEncontradoException($"NO SE ECONTRO EL TICKET CON ID DE {id}");
        }
        return ticket;
    }

    public List<Ticket> ListarTicketsDeCliente(int clienteId)
    {
        if (_clienteRepository.ObtenerPorId(clienteId) == null)
        {
            throw new NoEncontradoException($"NO EXISTE EL CLIENTE CON ID {clienteId}");
        }

        return _ticketRepository.ObtenerPorCliente(clienteId);
    }

    public Ticket Crear(TicketRequestDto dto)
    {
        ValidarDatos(dto, idActual: 0);

        Ticket nuevo = new Ticket{
             // EL ID LO GENERA EL MISMO SLQITE
             Estado = "Activo"
            };

        CopiarDatos(dto, nuevo); // Le pasa el cuerpo y el id ya asignando

        _ticketRepository.Agregar(nuevo);

        return nuevo;
    }
    public Ticket Actualizar(int id, TicketRequestDto dto)
    {
        Ticket ticket = ObtenerPorId(id);

        ValidarDatos(dto, idActual: id);
        CopiarDatos(dto, ticket);
        _ticketRepository.Actualizar(ticket);

        return ticket;
    }
    public void Eliminar(int id)
    {
        Ticket ticket = ObtenerPorId(id);
        _ticketRepository.Eliminar(ticket);
    }

    private void ValidarDatos(TicketRequestDto dto, int idActual)
    {
        if (_clienteRepository.ObtenerPorId(dto.ClienteId) == null)
        {
            throw new NoEncontradoException($"NO EXISTE EL CLIENTE CON ID {dto.ClienteId}");
        }

        if (_peliculaRepository.ObtenerPorId(dto.PeliculaId) == null)
        {
            throw new NoEncontradoException($"NO EXISTE LA PELICULA CON ID   {dto.PeliculaId}");
        }
        // EL ASIENTO ESTA OCUPADO SI YA HAY UN TICKET ACTIVO CON LA MISMA PELICULA , FECHA , SALA Y ASIENTO
        bool asientoOcupado = _ticketRepository.ObtenerTodos().Any(t =>
            t.Id != idActual &&
            t.Estado == "Activo" &&
            t.PeliculaId == dto.PeliculaId &&
            t.FechaFuncion == dto.FechaFuncion &&
            t.Sala.Equals(dto.Sala, StringComparison.OrdinalIgnoreCase) &&
            t.Asiento.Equals(dto.Asiento, StringComparison.OrdinalIgnoreCase));

        if (asientoOcupado)
        {
            throw new ConflictoException($"EL ASIENTO {dto.Asiento.ToUpper()} YA ESTA VENDIDO PARA OTRA FUNCION.");
        }
    }
    // PASA LOS DATOS DEL DTO ( LO QUE ENVIA EL CLIENTE ) AL TICKET ( LO QUE SE GUARDA EN LA BASE DE DATOS )
    private static void CopiarDatos(TicketRequestDto dto, Ticket ticket)
    {
        ticket.ClienteId = dto.ClienteId;
        ticket.PeliculaId = dto.PeliculaId;
        ticket.FechaFuncion = dto.FechaFuncion!.Value;
        ticket.Sala = dto.Sala;
        ticket.Asiento = dto.Asiento.ToUpper(); // PASARL EN MAYUSCULA
        ticket.Precio = dto.Precio;
    }
}
