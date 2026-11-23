using API_CINE.Data;
using API_CINE.Models;

namespace API_CINE.Repository;

// REPOSITORIO PARA LAS CONSULTAS DE LAS PELICULAS

// IMPLEMENTA IPELICULAREPOSITORY , PARA TENER LOS METODOS DEFINIDOS EN EL CONTRATO
public class PeliculaRepository(AppDbContext _contexto) : IPeliculaRepository
{

    public List<Pelicula> ObtenerTodos()
    {
        return _contexto.Peliculas.ToList();
    }

    // USAMOS METODOS LINQ
    public Pelicula? ObtenerPorId(int id)
    {
        return _contexto.Peliculas.FirstOrDefault(p => p.Id == id);
    }

    public void Agregar(Pelicula pelicula)
    {
        _contexto.Peliculas.Add(pelicula);
        _contexto.SaveChanges();
    }

    public void Actualizar(Pelicula pelicula)
    {
        _contexto.Peliculas.Update(pelicula);
        _contexto.SaveChanges();
    }

    public void Eliminar(Pelicula pelicula)
    {
        _contexto.Peliculas.Remove(pelicula);
        _contexto.SaveChanges();
    }
}
