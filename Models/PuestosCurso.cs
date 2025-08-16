using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class PuestosCurso
{
    [JsonPropertyName("positionCourseId")]
    public int IdPuestoCurso { get; set; }

    [JsonPropertyName("positionId")]
    public int? KPuesto { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }
}

public class PuestosCursoDto
{
    [JsonPropertyName("positionCourseId")]
    public int IdPuestoCurso { get; set; }

    [JsonPropertyName("positionId")]
    public int? KPuesto { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }
}