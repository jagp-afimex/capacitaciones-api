using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Video
{
    [JsonPropertyName("videoId")]
    public int IdVideo { get; set; }

    [JsonPropertyName("videoName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    [JsonPropertyName("videoReference")]
    public string? Referencia { get; set; }

    public int? Duracion { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual Seccion? IdSeccionNavigation { get; set; }

    [JsonPropertyName("order")]
    public int? Orden { get; set; }
}

public class VideoDto
{
    [JsonPropertyName("videoId")]
    public int IdVideo { get; set; }

    [JsonPropertyName("videoName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("sectionId")]
    public int? IdSeccion { get; set; }

    [JsonPropertyName("videoReference")]
    public string? Referencia { get; set; }

    [JsonPropertyName("duration")]
    public int? Duracion { get; set; }

    [JsonPropertyName("order")]
    public int? Orden { get; set; }
}