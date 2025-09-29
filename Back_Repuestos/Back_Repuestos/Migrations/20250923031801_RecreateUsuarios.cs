using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class RecreateUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoExpira",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoVerificacion",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoExpira",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoVerificacion",
                table: "Usuarios");
        }
    }
}
