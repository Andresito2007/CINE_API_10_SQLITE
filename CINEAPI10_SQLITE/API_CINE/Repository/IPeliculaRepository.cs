using API_CINE.Models;

namespace API_CINE.Repository;

// CONTRATO DEL REPOSITORIO DE PELICULAS : SOLO ACCESO A CONSULTAR Y MODIFICAR LOS DATOS DE LAS PELICULAS
public interface IPeliculaRepository
{
    List<Pelicula> ObtenerTodos();
    Pelicula? ObtenerPorId(int id);

    // FUNCIONALIDADES FUTURAS A USAR EN EL CONTROLLER
    void Agregar(Pelicula pelicula);

    void Actualizar(Pelicula pelicula);

    void Eliminar(Pelicula pelicula);
}
