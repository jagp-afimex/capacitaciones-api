using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class AvancesCurso
{
    [JsonPropertyName("progressId")]
    public int IdAvance { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    [JsonPropertyName("videoId")]
    public int? IdVideo { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("stateId")]
    public int? IdEstado { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Fecha { get; set; }

    [JsonPropertyName("version")]
    public int? VersionCurso { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }

    public virtual Video? IdVideoNavigation { get; set; }
}

public class AvancesCursoDto
{
    [JsonPropertyName("progressId")]
    public int IdAvance { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    [JsonPropertyName("videoId")]
    public int? IdVideo { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("stateId")]
    public int? IdEstado { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Fecha { get; set; }

    [JsonPropertyName("version")]
    public int? VersionCurso { get; set; }
}
