using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capacitaciones_api.Migrations
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
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__8A3D240C0D892E29", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    idEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estados__62EA894AA6FDF85B", x => x.idEstado);
                });

            migrationBuilder.CreateTable(
                name: "TiposPreguntas",
                columns: table => new
                {
                    idTipoPregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposPre__F3A2EBA797956516", x => x.idTipoPregunta);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                columns: table => new
                {
                    idTipoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposUsu__03006BFF7B9A4D00", x => x.idTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    idCurso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    idCategoria = table.Column<int>(type: "int", nullable: true),
                    PortadaReferencia = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cursos__8551ED05DAE0FEB4", x => x.idCurso);
                    table.ForeignKey(
                        name: "FK_Curso_Categoria",
                        column: x => x.idCategoria,
                        principalTable: "Categorias",
                        principalColumn: "idCategoria");
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    idInscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCurso = table.Column<int>(type: "int", nullable: true),
                    K_Empleado = table.Column<int>(type: "int", nullable: true),
                    Calificacion = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inscripc__3D58AB69DE6F91C6", x => x.idInscripcion);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Curso",
                        column: x => x.idCurso,
                        principalTable: "Cursos",
                        principalColumn: "idCurso");
                });

            migrationBuilder.CreateTable(
                name: "PuestosCursos",
                columns: table => new
                {
                    idPuestoCurso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    K_Puesto = table.Column<int>(type: "int", nullable: true),
                    idCurso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PuestosC__501B70AF9767CAF5", x => x.idPuestoCurso);
                    table.ForeignKey(
                        name: "FK_PuestosCursos_Curso",
                        column: x => x.idCurso,
                        principalTable: "Cursos",
                        principalColumn: "idCurso");
                });

            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    idSeccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(220)", unicode: false, maxLength: 220, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    idCurso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seccione__94B87A7C3B326106", x => x.idSeccion);
                    table.ForeignKey(
                        name: "FK_Seccion_Curso",
                        column: x => x.idCurso,
                        principalTable: "Cursos",
                        principalColumn: "idCurso");
                });

            migrationBuilder.CreateTable(
                name: "Evaluaciones",
                columns: table => new
                {
                    idEvaluacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSeccion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluaci__0E50CD2751910E8E", x => x.idEvaluacion);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Seccion",
                        column: x => x.idSeccion,
                        principalTable: "Secciones",
                        principalColumn: "idSeccion");
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    idVideo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    idSeccion = table.Column<int>(type: "int", nullable: true),
                    Referencia = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true),
                    Duracion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Videos__D2D0AD2AA93D8AB3", x => x.idVideo);
                    table.ForeignKey(
                        name: "FK_Video_Seccion",
                        column: x => x.idSeccion,
                        principalTable: "Secciones",
                        principalColumn: "idSeccion");
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    idPregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextoPregunta = table.Column<string>(type: "varchar(280)", unicode: false, maxLength: 280, nullable: true),
                    idTipoPregunta = table.Column<int>(type: "int", nullable: true),
                    idEvaluacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pregunta__623EEC4278A0C9A8", x => x.idPregunta);
                    table.ForeignKey(
                        name: "FK_Pregunta_Evaluacion",
                        column: x => x.idEvaluacion,
                        principalTable: "Evaluaciones",
                        principalColumn: "idEvaluacion");
                    table.ForeignKey(
                        name: "FK_Pregunta_TipoPregunta",
                        column: x => x.idTipoPregunta,
                        principalTable: "TiposPreguntas",
                        principalColumn: "idTipoPregunta");
                });

            migrationBuilder.CreateTable(
                name: "AvancesCurso",
                columns: table => new
                {
                    idAvance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCurso = table.Column<int>(type: "int", nullable: true),
                    idSeccion = table.Column<int>(type: "int", nullable: true),
                    idVideo = table.Column<int>(type: "int", nullable: true),
                    K_Empleado = table.Column<int>(type: "int", nullable: true),
                    idEstado = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    VersionCurso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AvancesC__B1657DC66A9225A1", x => x.idAvance);
                    table.ForeignKey(
                        name: "FK_AvanceCurso_Curso",
                        column: x => x.idCurso,
                        principalTable: "Cursos",
                        principalColumn: "idCurso");
                    table.ForeignKey(
                        name: "FK_AvanceCurso_Estado",
                        column: x => x.idEstado,
                        principalTable: "Estados",
                        principalColumn: "idEstado");
                    table.ForeignKey(
                        name: "FK_AvanceCurso_Seccion",
                        column: x => x.idSeccion,
                        principalTable: "Secciones",
                        principalColumn: "idSeccion");
                    table.ForeignKey(
                        name: "FK_AvanceCurso_Video",
                        column: x => x.idVideo,
                        principalTable: "Videos",
                        principalColumn: "idVideo");
                });

            migrationBuilder.CreateTable(
                name: "OpcionesPreguntas",
                columns: table => new
                {
                    idOpcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opcion = table.Column<string>(type: "varchar(220)", unicode: false, maxLength: 220, nullable: true),
                    EsRespuesta = table.Column<bool>(type: "bit", nullable: true),
                    idPregunta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Opciones__A914DF355F8AF80E", x => x.idOpcion);
                    table.ForeignKey(
                        name: "FK_OpcionesPregunta_Pregunta",
                        column: x => x.idPregunta,
                        principalTable: "Preguntas",
                        principalColumn: "idPregunta");
                });

            migrationBuilder.CreateTable(
                name: "RespuestasPreguntas",
                columns: table => new
                {
                    idRespuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Respuesta = table.Column<string>(type: "varchar(280)", unicode: false, maxLength: 280, nullable: true),
                    K_Empleado = table.Column<int>(type: "int", nullable: true),
                    idPregunta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Respuest__8AB5BFC8EA242D0C", x => x.idRespuesta);
                    table.ForeignKey(
                        name: "FK_RespuestasPregunta_Pregunta",
                        column: x => x.idPregunta,
                        principalTable: "Preguntas",
                        principalColumn: "idPregunta");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvancesCurso_idCurso",
                table: "AvancesCurso",
                column: "idCurso");

            migrationBuilder.CreateIndex(
                name: "IX_AvancesCurso_idEstado",
                table: "AvancesCurso",
                column: "idEstado");

            migrationBuilder.CreateIndex(
                name: "IX_AvancesCurso_idSeccion",
                table: "AvancesCurso",
                column: "idSeccion");

            migrationBuilder.CreateIndex(
                name: "IX_AvancesCurso_idVideo",
                table: "AvancesCurso",
                column: "idVideo");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_idCategoria",
                table: "Cursos",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_idSeccion",
                table: "Evaluaciones",
                column: "idSeccion");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_idCurso",
                table: "Inscripciones",
                column: "idCurso");

            migrationBuilder.CreateIndex(
                name: "IX_OpcionesPreguntas_idPregunta",
                table: "OpcionesPreguntas",
                column: "idPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_idEvaluacion",
                table: "Preguntas",
                column: "idEvaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_idTipoPregunta",
                table: "Preguntas",
                column: "idTipoPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_PuestosCursos_idCurso",
                table: "PuestosCursos",
                column: "idCurso");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasPreguntas_idPregunta",
                table: "RespuestasPreguntas",
                column: "idPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_idCurso",
                table: "Secciones",
                column: "idCurso");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_idSeccion",
                table: "Videos",
                column: "idSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvancesCurso");

            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "OpcionesPreguntas");

            migrationBuilder.DropTable(
                name: "PuestosCursos");

            migrationBuilder.DropTable(
                name: "RespuestasPreguntas");

            migrationBuilder.DropTable(
                name: "TiposUsuario");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Evaluaciones");

            migrationBuilder.DropTable(
                name: "TiposPreguntas");

            migrationBuilder.DropTable(
                name: "Secciones");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
