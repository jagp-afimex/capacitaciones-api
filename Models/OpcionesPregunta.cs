using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class OpcionesPregunta
{
    public int IdOpcion { get; set; }

    public string? Opcion { get; set; }

    public bool? EsRespuesta { get; set; }

    public int? IdPregunta { get; set; }

    public virtual Pregunta? IdPreguntaNavigation { get; set; }
}
