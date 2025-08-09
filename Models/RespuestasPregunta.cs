using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class RespuestasPregunta
{
    public int IdRespuesta { get; set; }

    public string? Respuesta { get; set; }

    public int? KEmpleado { get; set; }

    public int? IdPregunta { get; set; }

    public virtual Pregunta? IdPreguntaNavigation { get; set; }
}
