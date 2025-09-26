using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Models;

public partial class CapacitacionesPruebasContext : DbContext
{
    public CapacitacionesPruebasContext(DbContextOptions<CapacitacionesPruebasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvancesCurso> AvancesCursos { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Evaluacion> Evaluaciones { get; set; }

    public virtual DbSet<Inscripcion> Inscripciones { get; set; }

    public virtual DbSet<OpcionesPregunta> OpcionesPreguntas { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<PuestosCurso> PuestosCursos { get; set; }

    public virtual DbSet<RespuestasPregunta> RespuestasPreguntas { get; set; }

    public virtual DbSet<Seccion> Secciones { get; set; }

    public virtual DbSet<TiposPregunta> TiposPreguntas { get; set; }

    public virtual DbSet<TiposUsuario> TiposUsuarios { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<EvaluacionRevisada> EvaluacionesRevisadas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvancesCurso>(entity =>
        {
            entity.HasKey(e => e.IdAvance).HasName("PK__AvancesC__B1657DC66A9225A1");

            entity.ToTable("AvancesCurso");

            entity.Property(e => e.IdAvance).HasColumnName("idAvance");
            entity.Property(e => e.Fecha).HasColumnType("smalldatetime");
            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdSeccion).HasColumnName("idSeccion");
            entity.Property(e => e.IdVideo).HasColumnName("idVideo");
            entity.Property(e => e.KEmpleado).HasColumnName("K_Empleado");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.AvancesCursos)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK_AvanceCurso_Curso");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.AvancesCursos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK_AvanceCurso_Estado");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.AvancesCursos)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_AvanceCurso_Seccion");

            entity.HasOne(d => d.IdVideoNavigation).WithMany(p => p.AvancesCursos)
                .HasForeignKey(d => d.IdVideo)
                .HasConstraintName("FK_AvanceCurso_Video");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__8A3D240C0D892E29");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Nombre");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__Cursos__8551ED05DAE0FEB4");

            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.FechaModificacion).HasColumnType("smalldatetime");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PortadaReferencia)
                .HasMaxLength(800)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_Curso_Categoria");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estados__62EA894AA6FDF85B");

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Nombre");
        });

        modelBuilder.Entity<Evaluacion>(entity =>
        {
            entity.HasKey(e => e.IdEvaluacion).HasName("PK__Evaluaci__0E50CD2751910E8E");

            entity.Property(e => e.IdEvaluacion).HasColumnName("idEvaluacion");
            entity.Property(e => e.IdSeccion).HasColumnName("idSeccion");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Evaluaciones)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_Evaluacion_Seccion");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.IdInscripcion).HasName("PK__Inscripc__3D58AB69DE6F91C6");

            entity.Property(e => e.IdInscripcion).HasColumnName("idInscripcion");
            entity.Property(e => e.Calificacion).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.KEmpleado).HasColumnName("K_Empleado");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK_Inscripcion_Curso");
        });

        modelBuilder.Entity<OpcionesPregunta>(entity =>
        {
            entity.HasKey(e => e.IdOpcion).HasName("PK__Opciones__A914DF355F8AF80E");

            entity.Property(e => e.IdOpcion).HasColumnName("idOpcion");
            entity.Property(e => e.IdPregunta).HasColumnName("idPregunta");
            entity.Property(e => e.Opcion)
                .HasMaxLength(220)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.OpcionesPregunta)
                .HasForeignKey(d => d.IdPregunta)
                .HasConstraintName("FK_OpcionesPregunta_Pregunta");
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PK__Pregunta__623EEC4278A0C9A8");

            entity.Property(e => e.IdPregunta).HasColumnName("idPregunta");
            entity.Property(e => e.IdEvaluacion).HasColumnName("idEvaluacion");
            entity.Property(e => e.IdTipoPregunta).HasColumnName("idTipoPregunta");
            entity.Property(e => e.TextoPregunta)
                .HasMaxLength(280)
                .IsUnicode(false)
                .HasColumnName("TextoPregunta");

            entity.HasOne(d => d.IdEvaluacionNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.IdEvaluacion)
                .HasConstraintName("FK_Pregunta_Evaluacion");

            entity.HasOne(d => d.IdTipoPreguntaNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.IdTipoPregunta)
                .HasConstraintName("FK_Pregunta_TipoPregunta");
        });

        modelBuilder.Entity<PuestosCurso>(entity =>
        {
            entity.HasKey(e => e.IdPuestoCurso).HasName("PK__PuestosC__501B70AF9767CAF5");

            entity.Property(e => e.IdPuestoCurso).HasColumnName("idPuestoCurso");
            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.KPuesto).HasColumnName("K_Puesto");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.PuestosCursos)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK_PuestosCursos_Curso");
        });

        modelBuilder.Entity<RespuestasPregunta>(entity =>
        {
            entity.HasKey(e => e.IdRespuesta).HasName("PK__Respuest__8AB5BFC8EA242D0C");

            entity.Property(e => e.IdRespuesta).HasColumnName("idRespuesta");
            entity.Property(e => e.IdPregunta).HasColumnName("idPregunta");
            entity.Property(e => e.KEmpleado).HasColumnName("K_Empleado");
            entity.Property(e => e.Respuesta)
                .HasMaxLength(280)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.RespuestasPregunta)
                .HasForeignKey(d => d.IdPregunta)
                .HasConstraintName("FK_RespuestasPregunta_Pregunta");
        });

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.IdSeccion).HasName("PK__Seccione__94B87A7C3B326106");

            entity.Property(e => e.IdSeccion).HasColumnName("idSeccion");
            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(220)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Secciones)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK_Seccion_Curso");
        });

        modelBuilder.Entity<TiposPregunta>(entity =>
        {
            entity.HasKey(e => e.IdTipoPregunta).HasName("PK__TiposPre__F3A2EBA797956516");

            entity.Property(e => e.IdTipoPregunta).HasColumnName("idTipoPregunta");
            entity.Property(e => e.Tipo)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TiposUsu__03006BFF7B9A4D00");

            entity.ToTable("TiposUsuario");

            entity.Property(e => e.IdTipoUsuario).HasColumnName("idTipoUsuario");
            entity.Property(e => e.Tipo)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.IdVideo).HasName("PK__Videos__D2D0AD2AA93D8AB3");

            entity.Property(e => e.IdVideo).HasColumnName("idVideo");
            entity.Property(e => e.IdSeccion).HasColumnName("idSeccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Referencia)
                .HasMaxLength(800)
                .IsUnicode(false);

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Videos)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK_Video_Seccion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
