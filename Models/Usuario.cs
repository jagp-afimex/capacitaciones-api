using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public class Usuario
{
    [JsonPropertyName("userId")]
    public int IdUsuario { get; set; }
    [JsonPropertyName("employeeId")]
    public int IdEmpleado { get; set; }
    [JsonPropertyName("Name")]
    public string? Empleado { get; set; }
    [JsonPropertyName("positionId")]
    public int IdPuesto { get; set; }
    [JsonPropertyName("positionName")]
    public string? Puesto { get; set; }
    [JsonPropertyName("picture")]
    public string? Foto { get; set; }

    
}

