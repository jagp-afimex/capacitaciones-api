using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class OpcionesPregunta
{
    [JsonPropertyName("optionId")]
    public int IdOpcion { get; set; }

    [JsonPropertyName("option")]
    public string? Opcion { get; set; }

    [JsonPropertyName("isCorrectAnswer")]
    public bool? EsRespuesta { get; set; }

    [JsonPropertyName("questionId")]
    public int? IdPregunta { get; set; }

    public virtual Pregunta? IdPreguntaNavigation { get; set; }
}

public class OpcionesPreguntaDto
{
    [JsonPropertyName("optionId")]
    public int IdOpcion { get; set; }

    [JsonPropertyName("option")]
    public string? Opcion { get; set; }

    [JsonPropertyName("isCorrectAnswer")]
    public bool? EsRespuesta { get; set; }

    [JsonPropertyName("questionId")]
    public int? IdPregunta { get; set; }
    
}