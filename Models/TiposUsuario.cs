using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class TiposUsuario
{
    [JsonPropertyName("userTypeId")]
    public int IdTipoUsuario { get; set; }

    [JsonPropertyName("userType")]
    public string? Tipo { get; set; }
}
