using API_CINE.Middleware;
using API_CINE.Data;
using API_CINE.Repository;
using API_CINE.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

// NUESTRA APP
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// REGISTRAMOS NUESTRA BD
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddIdentityCore<IdentityUser>(opciones =>
{
    opciones.Password.RequiredLength = 6;
    opciones.Password.RequireNonAlphanumeric = false; // no pide simbolos
    opciones.Password.RequireUppercase = false;
    opciones.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();


// INYECCION DE DEPENDENCIAS
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();

// SE REGISTRAN DIRECTAMENTE
builder.Services.AddScoped<ServicioAutenticacion>();

builder.Services.AddScoped<ServicioToken>();

// CONFIGURACION JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opciones =>
{
    // TokenValidationParameters : LAS REGLAS QUE DEBE CUMPLIR UN TOKEN PARA SER ACEPTADO
    opciones.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Emisor"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audiencia"],

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Clave"]!)),

        ClockSkew = TimeSpan.Zero,

        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };
});

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<ManejadorExcepciones>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// MIGRACIONES Y SEMILLA DE ROLES Y ADMIN
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    contexto.Database.Migrate();

    // CREAR ROLES
    var gestorRoles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (string rol in new[] { "admin", "usuario" })
    {
        if (!await gestorRoles.RoleExistsAsync(rol))
        {
            await gestorRoles.CreateAsync(new IdentityRole(rol));
        }
    }

    // CREAR ADMIN DE PRUEBA

    var gestorUsuarios = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if (await gestorUsuarios.FindByEmailAsync("admin@cine.com") == null)
    {
        IdentityUser admin = new()
        {
            UserName = "admin@cine.com",
            Email = "admin@cine.com"
        };

        IdentityResult creado = await gestorUsuarios.CreateAsync(admin, "Admin123");

        if (creado.Succeeded)
        {
            await gestorUsuarios.AddToRoleAsync(admin, "admin");
        }
    }
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

