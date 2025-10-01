using System.ComponentModel.DataAnnotations;

namespace capacitaciones_api.Models;

public partial class EmpleadoRegistrado
{
    [Key]
    public int IdRegistro { get; set; }

    public int IdEmpleado { get; set; }

    public int IdTipoUsuario { get; set; }
}