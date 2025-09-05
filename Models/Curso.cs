using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    [JsonPropertyName("courseName")]
    public string? Nombre { get; set; }

    public int? Version { get; set; }

    public DateTime? FechaModificacion { get; set; }

    [JsonPropertyName("categoryId")]
    public int? IdCategoria { get; set; }

    [JsonPropertyName("coverReference")]
    public string? PortadaReferencia { get; set; }

    [JsonPropertyName("courseProgress")]
    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    [JsonPropertyName("enrollments")]
    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

    [JsonPropertyName("coursePositions")]
    public virtual ICollection<PuestosCurso> PuestosCursos { get; set; } = new List<PuestosCurso>();

    [JsonPropertyName("sections")]
    public virtual ICollection<Seccion> Secciones { get; set; } = new List<Seccion>();

    [JsonPropertyName("courseDescription")]
    public string? Descripcion { get; set; }
}

public class CursoDto
{
    [JsonPropertyName("courseId")]
    public int IdCurso { get; set; }

    [JsonPropertyName("courseName")]
    public string? Nombre { get; set; }
    
    [JsonPropertyName("courseDescription")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("version")]
    public int? Version { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? FechaModificacion { get; set; }

    [JsonPropertyName("categoryId")]
    public int? IdCategoria { get; set; }

    [JsonPropertyName("coverReference")]
    public string? PortadaReferencia { get; set; }

    [JsonPropertyName("category")]
    public CategoriaDto? Categoria { get; set; }

    [JsonPropertyName("sections")]
    public ICollection<SeccionDto> Secciones { get; set; } = [];

    [JsonPropertyName("enrollments")]
    public ICollection<InscripcionDto> Inscripciones { get; set; } = [];
}
