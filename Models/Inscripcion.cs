using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Inscripcion
{
    public int IdInscripcion { get; set; }

    public int? IdCurso { get; set; }

    public int? KEmpleado { get; set; }

    public decimal? Calificacion { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }
}
