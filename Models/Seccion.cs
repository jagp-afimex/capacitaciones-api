using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Seccion
{
    public int IdSeccion { get; set; }

    public string? Nombre { get; set; }

    public int? Orden { get; set; }

    public int? IdCurso { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
