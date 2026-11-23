using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API_CINE.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Dni = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Genero = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DuracionMin = table.Column<int>(type: "INTEGER", nullable: false),
                    Clasificacion = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeliculaId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaFuncion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sala = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Asiento = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Dni", "Email", "Nombre" },
                values: new object[,]
                {
                    { 1, "72451234", "Estefano@gmail.com", "Estefano" },
                    { 2, "70112233", "Rodrigo@gmail.com", "Rodrigo" },
                    { 3, "61226187", "Andresito@gmail.com", "Andres" }
                });

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Id", "Clasificacion", "DuracionMin", "Genero", "Titulo" },
                values: new object[,]
                {
                    { 1, "PG-13", 175, "Drama", "IDAT VS ZEGEL LA SECUELA" },
                    { 2, "PG-9", 195, "Comedia", "LAS AVENTURAS DE OCHUPE" },
                    { 3, "PG-18", 120, "Romance", "AMOR DE ESPIA Y CEO DE NVIDIA " }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Asiento", "ClienteId", "Estado", "FechaFuncion", "PeliculaId", "Precio", "Sala" },
                values: new object[,]
                {
                    { 1, "F7", 2, "Activo", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 10.0m, "Sala 1" },
                    { 2, "G8", 3, "Activo", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 30.0m, "Sala 6" },
                    { 3, "H9", 1, "Activo", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 20.0m, "Sala 7" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
