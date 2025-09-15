using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capacitaciones_api.Migrations
{
    /// <inheritdoc />
    public partial class OrdenyArchivado_columnas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Videos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archivado",
                table: "Cursos",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Archivado",
                table: "Cursos");
        }
    }
}
