# CINE API — Sistema de Venta de Entradas de Cine

API REST desarrollada en **C# y ASP.NET Core 10** para gestionar tickets de cine. El proyecto utiliza **SQLite y Entity Framework Core** para la persistencia de datos, una arquitectura por capas y autenticación/autorización mediante **ASP.NET Core Identity y JWT**.

# Integrantes

- Ochupe Ayma Estefano Alessandro
- Choqque Mejia Andres David
- Tomayquispe Amasifuen Rodrigo Isaias

# Objetivo general

Desarrollar una API RESTful para gestionar la venta de entradas de cine, aplicando buenas prácticas de programación en C#, separación de responsabilidades, validación de datos, persistencia relacional, manejo centralizado de excepciones y control de acceso mediante roles.

# Tecnologías utilizadas

| C# | Lenguaje de programación |
| .NET 10 / ASP.NET Core 10 | Framework de la API |
| Entity Framework Core 10 | ORM y acceso a datos |
| SQLite | Base de datos relacional |
| ASP.NET Core Identity | Gestión de usuarios, contraseñas y roles |
| JWT (JSON Web Token) | Autenticación basada en tokens |
| Postman | Pruebas de endpoints |
| Git y GitHub | Control de versiones |

# Gestión de tickets

- Registrar tickets.
- Consultar todos los tickets y consultar un ticket por ID.
- Actualizar y eliminar tickets.
- Consultar los tickets asociados a un cliente.
- Generar el identificador del ticket mediante la base de datos.
- Asignar el estado inicial `Activo`.
- Validar los datos recibidos.
- Comprobar que existan el cliente y la película asociados.
- Evitar la venta duplicada de un asiento para la misma película, fecha de función y sala, considerando tickets activos.

# Persistencia con SQLite y Entity Framework Core

- Uso de `AppDbContext` como contexto de acceso a datos.
- Definición de los conjuntos `DbSet<Cliente>`, `DbSet<Pelicula>` y `DbSet<Ticket>`.
- Configuración de claves primarias, propiedades obligatorias y longitudes máximas mediante `OnModelCreating`.
- Datos iniciales de clientes, películas y tickets mediante `HasData()`.
- Repositorios de clientes, películas y tickets implementados con Entity Framework Core y consultas LINQ.
- Aplicación automática de migraciones pendientes al iniciar la API mediante `Database.Migrate()`.

# Relaciones entre entidades

La entidad `Ticket` contiene las claves foráneas `ClienteId` y `PeliculaId`.

- **Cliente – Ticket (1:N):** un cliente puede tener varios tickets; cada ticket pertenece a un cliente. La relación utiliza la propiedad de navegación `Ticket.Cliente` y la colección `Cliente.Tickets`.
- **Pelicula – Ticket (1:N):** una película puede estar asociada a varios tickets; cada ticket referencia una película mediante `PeliculaId`. La relación está configurada mediante Fluent API, sin propiedad de navegación en los modelos actuales.
- Las relaciones utilizan `DeleteBehavior.Restrict`, para impedir que se elimine un cliente o una película mientras existan tickets relacionados.
- Se utiliza `[JsonIgnore]` en las propiedades de navegación del cliente para evitar ciclos al serializar las entidades a JSON.

# Migraciones

El proyecto incluye las siguientes migraciones:

1. `MigracionInicial`: crea las tablas iniciales de clientes, películas y tickets, junto con sus datos iniciales.
2. `AgregarTelefonitoCliente`: agrega el campo opcional `Telefono` a la tabla `Clientes`.
3. `RelacionTicketsClientes`: incorpora las claves foráneas de tickets hacia clientes y películas y sus índices.
4. `AgregarIdentity`: agrega las tablas necesarias para ASP.NET Core Identity, usuarios y roles.

Las migraciones se encuentran en `API_CINE/Migrations/`. La aplicación ejecuta `contexto.Database.Migrate()` durante el inicio para aplicar las migraciones pendientes.

# Autenticación y autorización

Se implementó autenticación mediante ASP.NET Core Identity y JWT:

- Registro de usuarios con correo y contraseña.
- Inicio de sesión y emisión de un token JWT.
- Validación de contraseña mediante Identity.
- Las contraseñas son administradas por Identity, que las almacena mediante hash.
- El token incluye identificador, nombre, correo y roles del usuario.
- Endpoint protegido para consultar la identidad del usuario autenticado.
- Roles implementados: `admin` y `usuario`.
- Los usuarios que se registran desde la API reciben el rol `usuario`.
- Al iniciar la aplicación se comprueba la existencia de los roles y se crea un administrador de prueba si todavía no existe.

**Administrador de prueba (solo desarrollo):**

- Correo: `admin@cine.com`
- Contraseña: `Admin123`

Estas credenciales están definidas en el código para facilitar las pruebas locales. Deben cambiarse o eliminarse antes de cualquier despliegue real.

# Manejo de excepciones

La aplicación utiliza `ManejadorExcepciones` como manejador centralizado y excepciones personalizadas:

| Excepción                        | Código HTTP | Uso                                    |
| -------------------------------- | ----------: | -------------------------------------- |
| `NoEncontradoException`          |         404 | Recurso inexistente                    |
| `ReglaNegocioException`          |         400 | Incumplimiento de una regla de negocio |
| `ConflictoException`             |         409 | Conflicto, como un asiento ocupado     |
| `CredencialesInvalidasException` |         401 | Correo o contraseña incorrectos        |
| `ArgumentException`              |         400 | Argumento inválido                     |
| Otras excepciones                |         500 | Error interno no previsto              |

## Arquitectura y estructura del proyecto

El proyecto separa las responsabilidades en controladores, servicios, repositorios, modelos, DTO, contexto de datos y componentes transversales.

```text
CINEAPI10_SQLITE/
├── README.md
└── API_CINE/
    ├── Controllers/
    │   ├── AuthController.cs
    │   └── TicketsController.cs
    ├── Data/
    │   └── AppDbContext.cs
    ├── Dtos/
    │   ├── ErrorResponse.cs
    │   ├── TicketRequestDto.cs
    │   ├── RegistroRequestDto.cs
    │   ├── LoginRequestDto.cs
    │   ├── TokenResponse.cs
    │   └── UsuarioResponse.cs
    ├── Exceptions/
    │   ├── ConflictoException.cs
    │   ├── NoEncontradoException.cs
    │   ├── ReglaNegocioException.cs
    │   └── CredencialesInvalidasException.cs
    ├── Middleware/
    │   └── ManejadorExcepciones.cs
    ├── Migrations/
    │   ├── MigracionInicial
    │   ├── AgregarTelefonitoCliente
    │   ├── RelacionTicketsClientes
    │   ├── AgregarIdentity
    │   └── AppDbContextModelSnapshot.cs
    ├── Models/
    │   ├── Cliente.cs
    │   ├── Pelicula.cs
    │   └── Ticket.cs
    ├── Repository/
    │   ├── IClienteRepository.cs
    │   ├── ClienteRepository.cs
    │   ├── IPeliculaRepository.cs
    │   ├── PeliculaRepository.cs
    │   ├── ITicketRepository.cs
    │   └── TicketRepository.cs
    ├── Services/
    │   ├── ITicketService.cs
    │   ├── TicketService.cs
    │   ├── AuthService.cs
    │   └── TokenService.cs
    ├── postman/
    │   └── COLECCION_CINEAPI_JWT.postman_collection.json
    ├── Properties/
    │   └── launchSettings.json
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── dotnet-tools.json
    └── API_CINE.csproj
```

# Responsabilidad de las capas

- **Controllers:** reciben las solicitudes HTTP, aplican los atributos de autorización y devuelven respuestas.
- **Services:** contienen la lógica de negocio, las validaciones y la coordinación de operaciones.
- **Repository:** encapsula las consultas y operaciones de persistencia sobre las entidades.
- **Data:** contiene `AppDbContext`, que representa la sesión de trabajo de Entity Framework Core con la base de datos.
- **Models:** representan las entidades `Cliente`, `Pelicula` y `Ticket`.
- **DTOs:** definen los datos que recibe la API y las estructuras de respuesta.
- **Exceptions:** contiene las excepciones personalizadas.
- **Middleware:** centraliza el tratamiento de errores.
- **Migrations:** contiene el historial de cambios de la estructura de la base de datos.
- **Program.cs:** configura SQLite, Identity, JWT, inyección de dependencias, migraciones y middleware.

# Flujo general de una solicitud

```text
Cliente HTTP / Postman
        |
        v
Autenticación JWT y autorización por roles
        |
        v
Controller
        |
        v
Service (reglas de negocio)
        |
        v
Repository (consultas y operaciones)
        |
        v
AppDbContext / Entity Framework Core
        |
        v
Base de datos SQLite
```

# Autenticación — `/api/auth`

| Método | Endpoint | Descripción | Acceso |
| POST | `/api/auth/registro` | Registrar un usuario nuevo | Público |
| POST | `/api/auth/login` | Iniciar sesión y obtener JWT | Público |
| GET | `/api/auth/yo` | Consultar los datos del usuario autenticado | Requiere token |

**Ejemplo de registro:**

```http
POST /api/auth/registro
Content-Type: application/json
```

```json
{
  "correo": "usuario@correo.com",
  "clave": "Usuario123"
}
```

**Ejemplo de inicio de sesión:**

```http
POST /api/auth/login
Content-Type: application/json
```

```json
{
  "correo": "usuario@correo.com",
  "clave": "Usuario123"
}
```

La respuesta del login contiene el token y su fecha de expiración. Para consumir rutas protegidas, enviar el token en el encabezado:

```http
Authorization: Bearer <token>
```

**Consultar el usuario autenticado:**

```http
GET /api/auth/yo
Authorization: Bearer <token>
```

# Tickets — `/api/tickets`

El controlador de tickets requiere autenticación. Los permisos se restringen según el rol:

| Método | Endpoint | Descripción | Rol permitido |

| GET | `/api/tickets` | Obtener todos los tickets | `admin`, `usuario` |
| GET | `/api/tickets/{id}` | Obtener un ticket por ID | `admin`, `usuario` |
| GET | `/api/tickets/{id}/ticketcliente` | Listar tickets de un cliente | `admin`, `usuario` |
| POST | `/api/tickets` | Registrar un ticket | `admin` |
| PUT | `/api/tickets/{id}` | Actualizar un ticket | `admin` |
| DELETE | `/api/tickets/{id}` | Eliminar un ticket | `admin` |

**Ejemplo de creación de ticket:**

```http
POST /api/tickets
Content-Type: application/json
Authorization: Bearer <token-admin>
```

```json
{
  "clienteId": 1,
  "peliculaId": 1,
  "fechaFuncion": "2026-11-25T19:30:00",
  "sala": "Sala 1",
  "asiento": "F7",
  "precio": 20.00
}
```

Los IDs deben corresponder a clientes y películas existentes. El asiento debe estar disponible para la misma película, fecha de función y sala.

# Validaciones y reglas de negocio

El DTO `TicketRequestDto` aplica las siguientes validaciones:

- `ClienteId`: debe ser mayor que cero.
- `PeliculaId`: debe ser mayor que cero.
- `FechaFuncion`: es obligatoria.
- `Sala`: debe tener entre 3 y 20 caracteres.
- `Asiento`: debe respetar el formato de una fila de A a L y un número del 1 al 20. Ejemplo: `F7`.
- `Precio`: debe estar entre S/ 5 y S/ 50.

Además, `TicketService` comprueba:

- Que el cliente exista.
- Que la película exista.
- Que no exista otro ticket activo con la misma película, fecha, sala y asiento.
- Que el ticket consultado exista antes de actualizarlo o eliminarlo.

# Base de datos y migraciones

La cadena de conexión se configura en `appsettings.json` mediante la clave `ConnectionStrings:Conexion`. El proyecto utiliza SQLite y el archivo de base de datos configurado actualmente se llama `AppDb.db`.

# Requisitos

- SDK de .NET 10.
- Visual Studio o Visual Studio Code.
- Acceso a una terminal.
- Postman para probar los endpoints.

# Descargar y preparar el proyecto

1. Clonar o descargar el repositorio desde GitHub.
2. Extraer el ZIP si se descargó como archivo comprimido.
3. Abrir una terminal en la carpeta `API_CINE`, donde se encuentra `API_CINE.csproj`.

# Restaurar dependencias

```bash
dotnet restore
```

# Restaurar las herramientas locales de .NET

El proyecto incluye `dotnet-tools.json` con la herramienta `dotnet-ef`.

```bash
dotnet tool restore
```

# Aplicar migraciones manualmente (opcional)

```bash
dotnet ef database update
```

También se puede revisar el historial con:

```bash
dotnet ef migrations list
```

# Compilar el proyecto

```bash
dotnet build
```

# Ejecutar la API

```bash
dotnet run
```

Durante el inicio, la aplicación ejecuta `Database.Migrate()` y comprueba que existan los roles `admin` y `usuario`. También crea el administrador de prueba si no existe.

La terminal mostrará las direcciones locales en las que se ejecuta la aplicación. Utilizar la dirección correspondiente para enviar solicitudes desde Postman.

**Nota sobre la base de datos:** `AppDb.db` contiene datos locales de SQLite. La carpeta `Migrations/` debe conservarse en el repositorio; el archivo de base de datos local no debe utilizarse como sustituto de las migraciones.

# Configuración de seguridad para ejecución local

La configuración JWT se encuentra bajo la sección `Jwt` de `appsettings.json`:

- `Clave`: clave utilizada para firmar y validar tokens.
- `Emisor`: emisor esperado del token.
- `Audiencia`: audiencia esperada.
- `MinutosExpiracion`: duración del token.

La clave incluida en la configuración de desarrollo no debe reutilizarse en producción. Para otros entornos, configurar una clave secreta propia mediante variables de entorno o un gestor de secretos. En .NET, la variable correspondiente a `Jwt:Clave` puede definirse como `Jwt__Clave`.

Las credenciales del administrador de prueba también deben cambiarse o retirarse antes de desplegar la aplicación.

# Pruebas con Postman

El proyecto incluye una colección de Postman:

`API_CINE/postman/COLECCION_CINEAPI_JWT.postman_collection.json`

Se recomienda probar el flujo de autenticación en este orden:

1. Registrar un usuario.
2. Iniciar sesión con ese usuario.
3. Copiar el JWT recibido.
4. Enviar el token como `Bearer Token` en las solicitudes protegidas.
5. Probar el acceso de un usuario con rol `usuario`.
6. Iniciar sesión con el administrador de prueba y verificar las operaciones restringidas a `admin`.
7. Probar las operaciones de tickets: GET, GET por ID, GET por cliente, POST, PUT y DELETE.
8. Comprobar errores de validación, recursos inexistentes y asientos ocupados.

# Códigos HTTP utilizados

- `200 OK`: consulta o actualización exitosa.
- `201 Created`: recurso creado correctamente.
- `204 No Content`: eliminación exitosa.
- `400 Bad Request`: datos inválidos o regla de negocio incumplida.
- `401 Unauthorized`: falta autenticación o las credenciales son incorrectas.
- `403 Forbidden`: el usuario está autenticado, pero no tiene el rol requerido.
- `404 Not Found`: recurso inexistente.
- `409 Conflict`: conflicto de negocio, por ejemplo un asiento ya ocupado.
- `500 Internal Server Error`: error interno no previsto.
