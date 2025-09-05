using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Seccion
{
    [JsonPropertyName("sectionId")]
    public int IdSeccion { get; set; }

    [JsonPropertyName("sectionName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("order")]
    public int? Orden { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();

    public virtual Curso? IdCursoNavigation { get; set; }

    [JsonPropertyName("videos")]
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}

public class SeccionDto
{
    [JsonPropertyName("sectionId")]
    public int IdSeccion { get; set; }

    [JsonPropertyName("sectionName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("order")]
    public int? Orden { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("videos")]
    public virtual ICollection<VideoDto> Videos { get; set; } = [];
}
