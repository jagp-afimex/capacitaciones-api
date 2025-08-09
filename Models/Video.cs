using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Video
{
    public int IdVideo { get; set; }

    public string? Nombre { get; set; }

    public int? IdSeccion { get; set; }

    public string? Referencia { get; set; }

    public int? Duracion { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual Seccion? IdSeccionNavigation { get; set; }
}
