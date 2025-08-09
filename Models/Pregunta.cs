using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Pregunta
{
    public int IdPregunta { get; set; }

    public string? TextoPregunta { get; set; }

    public int? IdTipoPregunta { get; set; }

    public int? IdEvaluacion { get; set; }

    public virtual Evaluacion? IdEvaluacionNavigation { get; set; }

    public virtual TiposPregunta? IdTipoPreguntaNavigation { get; set; }

    public virtual ICollection<OpcionesPregunta> OpcionesPregunta { get; set; } = new List<OpcionesPregunta>();

    public virtual ICollection<RespuestasPregunta> RespuestasPregunta { get; set; } = new List<RespuestasPregunta>();
}
