using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReWearWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenUrlToPrenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Prendas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Prendas",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Prendas",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Prendas",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Prendas",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Prendas",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenUrl",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Prendas");
        }
    }
}
