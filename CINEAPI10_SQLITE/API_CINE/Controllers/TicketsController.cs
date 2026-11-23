using API_CINE.Dtos;
using API_CINE.Models;
using API_CINE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_CINE.Controllers;

// EL CONTROLLER SOLO RECIBE LA PETICION Y DELEGA : LA LOGICA DE NEGOCIO VIVE EN EL SERVICIO

[ApiController]
[Route("api/tickets")]
[Authorize]

public class TicketsController : ControllerBase
{
    private ITicketService _service;

    // INYECCION DE DEPENDENCIAS : .NET ENTREGA LA IMPLEMENTACION REGISTRADA EN Program.cs ( TicketService )
    public TicketsController(ITicketService service)
    {
        _service = service;
    }

    [HttpGet]

    [Authorize(Roles = "admin,usuario")]
    public ActionResult<List<Ticket>> ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos()); // ok=200
    }

    [HttpGet("{id}")]

    [Authorize(Roles = "admin,usuario")]
    public ActionResult<Ticket> ObtenerPorId(int id)
    {
        return Ok(_service.ObtenerPorId(id));
    }

    [HttpGet("{id}/ticketscliente")]

    [Authorize(Roles = "admin,usuario")]
    public ActionResult<List<Ticket>> ListarTicketsDeCliente (int id)

    {

     return Ok(_service.ListarTicketsDeCliente(id));

    }

    [HttpPost]

    [Authorize(Roles = "admin")]
    public ActionResult<Ticket> Crear(TicketRequestDto dto)
    {
        Ticket creado = _service.Crear(dto);
        // devuelve el ticket creado y su ruta
        return CreatedAtAction("ObtenerPorId", new { id = creado.Id }, creado); // createdataction=201
    }

    [HttpPut("{id}")]

    [Authorize(Roles = "admin")]
    public ActionResult<Ticket> Actualizar(int id, TicketRequestDto dto)
    {
        return Ok(_service.Actualizar(id, dto));
    }

    [HttpDelete("{id}")]

    [Authorize(Roles = "admin")]
    public IActionResult Eliminar(int id)
    {
        _service.Eliminar(id);

        return NoContent();
    }
}
