using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Estado
{
    [JsonPropertyName("stateId")]
    public int IdEstado { get; set; }

    [JsonPropertyName("stateName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("courseProgress")]
    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();
}
