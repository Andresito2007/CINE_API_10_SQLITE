using API_CINE.Data;
using API_CINE.Models;

namespace API_CINE.Repository;

public class ClienteRepository(AppDbContext _contexto) : IClienteRepository
{

    public List<Cliente> ObtenerTodos()
    {
        return _contexto.Clientes.ToList();
    }

    // USAMOS METODOS LINQ
    public Cliente? ObtenerPorId(int id)
    {
        return _contexto.Clientes.FirstOrDefault(c => c.Id == id);
    }

    public void Agregar(Cliente cliente)
    {
        _contexto.Clientes.Add(cliente);
        _contexto.SaveChanges(); //  guardar los cambios en la base de datos
    }

    public void Actualizar(Cliente cliente)
    {
        _contexto.Clientes.Update(cliente);
        _contexto.SaveChanges();
    }

    public void Eliminar(Cliente cliente)
    {
        _contexto.Clientes.Remove(cliente);
        _contexto.SaveChanges();
    }
}
