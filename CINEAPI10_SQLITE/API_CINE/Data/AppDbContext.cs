using API_CINE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_CINE.Data;

// EL DBCONTEXT REPRESENTA LA CONEXION CON LA BASE DE DATOS

// HEREDA DE IDENTITYDBCONTEXT Y TRAE LOS 7 TABLAS CON LOS ROLES Y USERS
public class AppDbContext(DbContextOptions<AppDbContext> opciones) : IdentityDbContext<IdentityUser>(opciones)
{

    // CADA DBSET ES UNA TABLA DENTRO DE NUESTRA BD
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OBLIGATORIO , CONFIGURA MOS LAS TABLAS
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(entidad =>
        {
            entidad.HasKey(c => c.Id);

            entidad.Property(c => c.Nombre).HasMaxLength(100).IsRequired();

            entidad.Property(c => c.Dni).HasMaxLength(8).IsRequired();

            entidad.Property(c => c.Email).HasMaxLength(100).IsRequired();

            // EL CAMPO TELEFONO ES OPCIONAL POR LO TRABAJADO CON 1ERA MIGRACION
            entidad.Property(c => c.Telefono).HasMaxLength(15);

            entidad.HasData(
                new Cliente(1, "Estefano", "72451234","Estefano@gmail.com"),
                new Cliente(2, "Rodrigo", "70112233","Rodrigo@gmail.com"),
                new Cliente(3, "Andres", "61226187","Andresito@gmail.com")
            );
        });

        modelBuilder.Entity<Pelicula>(entidad =>
        {
           entidad.HasKey(p => p.Id);

           entidad.Property(p => p.Titulo).HasMaxLength(150).IsRequired();

           entidad.Property(p => p.Genero).HasMaxLength(50).IsRequired();

           entidad.Property(p => p.DuracionMin).IsRequired();

           entidad.Property(p => p.Clasificacion).HasMaxLength(10).IsRequired();

           entidad.HasData(
                new Pelicula(1, "IDAT VS ZEGEL LA SECUELA", "Drama", 175, "PG-13"),
                new Pelicula(2, "LAS AVENTURAS DE OCHUPE", "Comedia", 195, "PG-9"),
                new Pelicula(3, "AMOR DE ESPIA Y CEO DE NVIDIA ", "Romance", 120, "PG-18")
            );
        });

        modelBuilder.Entity<Ticket>(entidad =>
        {
           entidad.HasKey(p => p.Id);

           entidad.Property(p => p.ClienteId).IsRequired();

           entidad.Property(p => p.PeliculaId).IsRequired();

           entidad.Property(p => p.FechaFuncion).IsRequired();

           entidad.Property(p => p.Sala).HasMaxLength(20).IsRequired();

           entidad.Property(p => p.Asiento).HasMaxLength(10).IsRequired();

           // EL DECIMAL EN SQLITE SE GUARDA COMO TEXT
           entidad.Property(p => p.Precio).IsRequired();

           entidad.Property(p => p.Estado).HasMaxLength(20).IsRequired();

           entidad.HasOne(t => t.Cliente)
                 .WithMany(c => c.Tickets)
                 .HasForeignKey(t => t.ClienteId)
                 .OnDelete(DeleteBehavior.Restrict);

           entidad.HasOne<Pelicula>()
                  .WithMany()
                  .HasForeignKey(t => t.PeliculaId)
                  .OnDelete(DeleteBehavior.Restrict);

           entidad.HasData(
                new Ticket(1, 2, 1, new DateTime(2026, 10, 15), "Sala 1", "F7", 10.0m),
                new Ticket(2, 3, 3, new DateTime(2026, 10, 20), "Sala 6", "G8", 30.0m),
                new Ticket(3, 1, 2, new DateTime(2026, 10, 12), "Sala 7", "H9", 20.0m)
            );
        });
    }
}
