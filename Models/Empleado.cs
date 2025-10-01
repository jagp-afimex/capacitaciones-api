
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;

namespace capacitaciones_api.Models;

public class Empleado
{
    [JsonPropertyName("employeeId")]
    public int IdEmpleado { get; set; }
    [JsonPropertyName("employeeName")]
    public string Nombre { get; set; }
    [JsonPropertyName("officeId")]
    public int IdOficina { get; set; }
    [JsonPropertyName("zone")]
    public string Region { get; set; }
    [JsonPropertyName("officeName")]
    public string Oficina { get; set; }
    [JsonPropertyName("positionId")]
    public int IdPuesto { get; set; }
    [JsonPropertyName("positionName")]
    public string Puesto { get; set; }
    [JsonPropertyName("payrollNumber")]
    public string NoNomina { get; set; }
    [JsonPropertyName("isActive")]
    public bool Activo { get; set; }
}

public class EmpleadoRepository
{
    readonly string _connectionString;

    public EmpleadoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");
    }

    public async Task<IEnumerable<Empleado>> Employees()
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Closed)
            connection.Open();

        IEnumerable<Empleado> empleados = await connection.QueryAsync<Empleado>("Empleados", null, null, 180, CommandType.StoredProcedure);
        return empleados;

    }

    public async Task<Empleado> EmployeeById(int? employeeId)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Closed)
            connection.Open();

        DynamicParameters parameters = new();
        parameters.Add("@idEmpleado", employeeId);

        return await connection.QueryFirstOrDefaultAsync<Empleado>("Empleados", parameters, null, 180, CommandType.StoredProcedure);
    }
}