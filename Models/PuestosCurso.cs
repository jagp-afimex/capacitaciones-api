using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class PuestosCurso
{
    public int IdPuestoCurso { get; set; }

    public int? KPuesto { get; set; }

    public int? IdCurso { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }
}
