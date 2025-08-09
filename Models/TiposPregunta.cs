using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class TiposPregunta
{
    public int IdTipoPregunta { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
