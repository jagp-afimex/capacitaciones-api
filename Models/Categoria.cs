using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Categoria
{
    [JsonPropertyName("categoryId")]
    public int IdCategoria { get; set; }

    [JsonPropertyName("categoryName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("courses")]
    public virtual ICollection<Curso> Cursos { get; set; } = [];

}
