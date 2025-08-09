using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Evaluacion
{
    public int IdEvaluacion { get; set; }

    public int? IdSeccion { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
