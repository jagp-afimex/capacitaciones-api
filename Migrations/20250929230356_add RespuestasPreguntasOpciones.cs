using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capacitaciones_api.Migrations
{
    /// <inheritdoc />
    public partial class addRespuestasPreguntasOpciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRevision",
                table: "RespuestasPreguntas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdRevision",
                table: "RespuestasPreguntas");
        }
    }
}
