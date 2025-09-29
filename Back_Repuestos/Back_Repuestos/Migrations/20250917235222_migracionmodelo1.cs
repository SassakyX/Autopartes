using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class migracionmodelo1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "categoria",
                table: "Productos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoria",
                table: "Productos");
        }
    }
}
