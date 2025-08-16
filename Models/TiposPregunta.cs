using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class TiposPregunta
{
    [JsonPropertyName("questionTypeId")]
    public int IdTipoPregunta { get; set; }

    [JsonPropertyName("questionType")]
    public string? Tipo { get; set; }

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}

public class TipoPreguntaDto
{
    [JsonPropertyName("questionTypeId")]
    public int IdTipoPregunta { get; set; }

    [JsonPropertyName("questionType")]
    public string? Tipo { get; set; }
}

