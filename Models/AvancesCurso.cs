using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class AvancesCurso
{
    public int IdAvance { get; set; }

    public int? IdCurso { get; set; }

    public int? IdSeccion { get; set; }

    public int? IdVideo { get; set; }

    public int? KEmpleado { get; set; }

    public int? IdEstado { get; set; }

    public DateTime? Fecha { get; set; }

    public int? VersionCurso { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }

    public virtual Video? IdVideoNavigation { get; set; }
}
