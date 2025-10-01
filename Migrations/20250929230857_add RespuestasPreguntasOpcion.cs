using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capacitaciones_api.Migrations
{
    /// <inheritdoc />
    public partial class addRespuestasPreguntasOpcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RespuestasPreguntaOpcion",
                columns: table => new
                {
                    IdRespuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPregunta = table.Column<int>(type: "int", nullable: false),
                    IdOpcionElegida = table.Column<int>(type: "int", nullable: false),
                    IdRevision = table.Column<int>(type: "int", nullable: false),
                    IdEvaluacionRevisadaNavigationIdRevision = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasPreguntaOpcion", x => x.IdRespuesta);
                    table.ForeignKey(
                        name: "FK_RespuestasPreguntaOpcion_EvaluacionesRevisadas_IdEvaluacionRevisadaNavigationIdRevision",
                        column: x => x.IdEvaluacionRevisadaNavigationIdRevision,
                        principalTable: "EvaluacionesRevisadas",
                        principalColumn: "IdRevision");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasPreguntaOpcion_IdEvaluacionRevisadaNavigationIdRevision",
                table: "RespuestasPreguntaOpcion",
                column: "IdEvaluacionRevisadaNavigationIdRevision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RespuestasPreguntaOpcion");
        }
    }
}
