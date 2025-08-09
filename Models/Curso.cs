using System;
using System.Collections.Generic;

namespace capacitaciones_api.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    public string? Nombre { get; set; }

    public int? Version { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? IdCategoria { get; set; }

    public string? PortadaReferencia { get; set; }

    public virtual ICollection<AvancesCurso> AvancesCursos { get; set; } = new List<AvancesCurso>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

    public virtual ICollection<PuestosCurso> PuestosCursos { get; set; } = new List<PuestosCurso>();

    public virtual ICollection<Seccion> Secciones { get; set; } = new List<Seccion>();
}
