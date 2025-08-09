using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();
}
