using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class resenas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalleVentas_Productos_IdProducto",
                table: "detalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_detalleVentas_Ventas_IdVenta",
                table: "detalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reseñas_Productos_ProductoId",
                table: "Reseñas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reseñas_Usuarios_UsuarioId",
                table: "Reseñas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_detalleVentas",
                table: "detalleVentas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reseñas",
                table: "Reseñas");

            migrationBuilder.RenameTable(
                name: "detalleVentas",
                newName: "DetalleVentas");

            migrationBuilder.RenameTable(
                name: "Reseñas",
                newName: "Resenas");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVentas_IdVenta",
                table: "DetalleVentas",
                newName: "IX_DetalleVentas_IdVenta");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVentas_IdProducto",
                table: "DetalleVentas",
                newName: "IX_DetalleVentas_IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Reseñas_UsuarioId",
                table: "Resenas",
                newName: "IX_Resenas_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Reseñas_ProductoId",
                table: "Resenas",
                newName: "IX_Resenas_ProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleVentas",
                table: "DetalleVentas",
                column: "IdDventa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resenas",
                table: "Resenas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Productos_IdProducto",
                table: "DetalleVentas",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Ventas_IdVenta",
                table: "DetalleVentas",
                column: "IdVenta",
                principalTable: "Ventas",
                principalColumn: "IdVenta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resenas_Productos_ProductoId",
                table: "Resenas",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resenas_Usuarios_UsuarioId",
                table: "Resenas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Productos_IdProducto",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Ventas_IdVenta",
                table: "DetalleVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Resenas_Productos_ProductoId",
                table: "Resenas");

            migrationBuilder.DropForeignKey(
                name: "FK_Resenas_Usuarios_UsuarioId",
                table: "Resenas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleVentas",
                table: "DetalleVentas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resenas",
                table: "Resenas");

            migrationBuilder.RenameTable(
                name: "DetalleVentas",
                newName: "detalleVentas");

            migrationBuilder.RenameTable(
                name: "Resenas",
                newName: "Reseñas");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVentas_IdVenta",
                table: "detalleVentas",
                newName: "IX_detalleVentas_IdVenta");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVentas_IdProducto",
                table: "detalleVentas",
                newName: "IX_detalleVentas_IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Resenas_UsuarioId",
                table: "Reseñas",
                newName: "IX_Reseñas_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Resenas_ProductoId",
                table: "Reseñas",
                newName: "IX_Reseñas_ProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_detalleVentas",
                table: "detalleVentas",
                column: "IdDventa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reseñas",
                table: "Reseñas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVentas_Productos_IdProducto",
                table: "detalleVentas",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVentas_Ventas_IdVenta",
                table: "detalleVentas",
                column: "IdVenta",
                principalTable: "Ventas",
                principalColumn: "IdVenta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reseñas_Productos_ProductoId",
                table: "Reseñas",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reseñas_Usuarios_UsuarioId",
                table: "Reseñas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
