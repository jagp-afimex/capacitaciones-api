using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class RespuestasPregunta
{
    [JsonPropertyName("answerId")]
    public int IdRespuesta { get; set; }

    [JsonPropertyName("answer")]
    public string? Respuesta { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("questionId")]
    public int? IdPregunta { get; set; }

    public virtual Pregunta? IdPreguntaNavigation { get; set; }

    public int? IdRevision { get; set; }
}

public class RespuestasPreguntaDto
{
    [JsonPropertyName("answerId")]
    public int IdRespuesta { get; set; }

    [JsonPropertyName("answer")]
    public string? Respuesta { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("questionId")]
    public int? IdPregunta { get; set; }
}
