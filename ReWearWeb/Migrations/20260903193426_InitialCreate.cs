using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReWearWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Talla = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstaDisponible = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prendas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prendas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Camisas casuales y formales", "Camisas" },
                    { 2, "Jeans, pantalones casuales y formales", "Pantalones" },
                    { 3, "Vestidos de todo tipo", "Vestidos" },
                    { 4, "Zapatillas, zapatos formales y casuales", "Zapatos" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellidos", "Email", "FechaRegistro", "Nombre", "Password", "Telefono" },
                values: new object[,]
                {
                    { 1, "García López", "maria@rewear.pe", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "María", "Rewear2024", "987654321" },
                    { 2, "Ramírez Paz", "carlos@rewear.pe", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos", "Rewear2024", "912345678" },
                    { 3, "Torres Quispe", "ana@rewear.pe", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana", "Rewear2024", "955111222" }
                });

            migrationBuilder.InsertData(
                table: "Prendas",
                columns: new[] { "Id", "CategoriaId", "Color", "Descripcion", "EstaDisponible", "Estado", "FechaPublicacion", "Marca", "Precio", "Talla", "Titulo", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "Blanco", "Camisa blanca de manga larga, talla M, pocas veces usada. Ideal para trabajo o eventos casuales.", true, "Usado - Bueno", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zara", 35.00m, "M", "Camisa blanca de algodón", 1 },
                    { 2, 2, "Azul oscuro", "Jeans azul oscuro, talla 32, corte recto. Muy cómodos y sin desgaste visible.", true, "Usado - Excelente", new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Levi's", 55.00m, "32", "Jeans azul clásico", 2 },
                    { 3, 3, "Estampado", "Vestido corto con estampado floral, material liviano. Perfecto para el verano.", true, "Nuevo con etiqueta", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mango", 48.00m, "S", "Vestido floral de verano", 3 },
                    { 4, 4, "Blanco", "Zapatillas blancas de cuero sintético, talla 40. Usadas solo un par de veces.", true, "Usado - Muy bueno", new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nike", 80.00m, "40", "Zapatillas blancas casuales", 1 },
                    { 5, 1, "Rojo/Negro", "Camisa de franela a cuadros rojos y negros, estilo vintage.", true, "Usado - Bueno", new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hollister", 42.00m, "L", "Camisa a cuadros estilo vintage", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prendas_CategoriaId",
                table: "Prendas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prendas_EstaDisponible",
                table: "Prendas",
                column: "EstaDisponible");

            migrationBuilder.CreateIndex(
                name: "IX_Prendas_UsuarioId",
                table: "Prendas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prendas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
