using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capacitaciones_api.Migrations
{
    /// <inheritdoc />
    public partial class addEvaluacionesRevisadasentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvaluacionesRevisadas",
                columns: table => new
                {
                    IdRevision = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KEmpleado = table.Column<int>(type: "int", nullable: false),
                    IdEvaluacion = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calificacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdEvaluacionNavigationIdEvaluacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionesRevisadas", x => x.IdRevision);
                    table.ForeignKey(
                        name: "FK_EvaluacionesRevisadas_Evaluaciones_IdEvaluacionNavigationIdEvaluacion",
                        column: x => x.IdEvaluacionNavigationIdEvaluacion,
                        principalTable: "Evaluaciones",
                        principalColumn: "idEvaluacion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesRevisadas_IdEvaluacionNavigationIdEvaluacion",
                table: "EvaluacionesRevisadas",
                column: "IdEvaluacionNavigationIdEvaluacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluacionesRevisadas");
        }
    }
}
