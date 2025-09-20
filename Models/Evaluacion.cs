using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Evaluacion
{
    [JsonPropertyName("evaluationId")]
    public int IdEvaluacion { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }

    [JsonPropertyName("questions")]
    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}

public class EvaluacionDto
{
    [JsonPropertyName("evaluationId")]
    public int IdEvaluacion { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    [JsonPropertyName("questions")]
    public virtual ICollection<PreguntaDto> Pregunta { get; set; } = [];

    [JsonPropertyName("section")]
    public virtual SeccionDto? Seccion { get; set; }

    // [JsonPropertyName("course")]
    // public virtual CursoDto? Curso { get; set; }
}