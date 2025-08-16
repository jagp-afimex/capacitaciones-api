using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class Inscripcion
{
    [JsonPropertyName("registrationId")]
    public int IdInscripcion { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("score")]
    public decimal? Calificacion { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }
}

public class InscripcionDto
{
    [JsonPropertyName("registrationId")]
    public int IdInscripcion { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("grade")]
    public decimal? Calificacion { get; set; }
}