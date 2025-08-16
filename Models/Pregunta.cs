using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Pregunta
{
    [JsonPropertyName("questionId")]
    public int IdPregunta { get; set; }

    [JsonPropertyName("question")]
    public string? TextoPregunta { get; set; }

    [JsonPropertyName("questionTypeId")]
    public int? IdTipoPregunta { get; set; }

    [JsonPropertyName("evaluationId")]
    public int? IdEvaluacion { get; set; }

    public virtual Evaluacion? IdEvaluacionNavigation { get; set; }

    public virtual TiposPregunta? IdTipoPreguntaNavigation { get; set; }

    [JsonPropertyName("options")]
    public virtual ICollection<OpcionesPregunta> OpcionesPregunta { get; set; } = new List<OpcionesPregunta>();

    [JsonPropertyName("answers")]
    public virtual ICollection<RespuestasPregunta> RespuestasPregunta { get; set; } = new List<RespuestasPregunta>();
}

public class PreguntaDto
{
    [JsonPropertyName("questionId")]
    public int IdPregunta { get; set; }

    [JsonPropertyName("question")]
    public string? TextoPregunta { get; set; }

    [JsonPropertyName("questionTypeId")]
    public int? IdTipoPregunta { get; set; }

    [JsonPropertyName("evaluationId")]
    public int? IdEvaluacion { get; set; }
}

public class PreguntaAbiertaDto : PreguntaDto
{
    [JsonPropertyName("answers")]
    public ICollection<RespuestasPregunta>? RespuestasPregunta { get; set; }
}

public class PreguntaOpcionesDto : PreguntaDto
{
    [JsonPropertyName("options")]
    public ICollection<OpcionesPregunta>? OpcionesPregunta { get; set; }
}