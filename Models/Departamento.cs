
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;

namespace capacitaciones_api.Models;

public class Departamento
{
    [JsonPropertyName("departmentId")]
    public int IdDepartamento { get; set; }

    [JsonPropertyName("departmentName")]
    public string Nombre { get; set; }
}

public class DepartamentoRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");

    public async Task<List<Departamento>> Departments()
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Open)
            connection.Open();

        List<Departamento> departments = (List<Departamento>)await connection.QueryAsync<Departamento>("Sl_Departamentos", commandTimeout: 120, commandType: CommandType.StoredProcedure);
        return departments;
    }

    public async Task<Departamento> DepartmentsById(int? departmentId)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Open)
            connection.Open();

        List<Departamento> departments = (List<Departamento>)await connection.QueryAsync<Puesto>("Sl_Departamentos", commandTimeout: 120, commandType: CommandType.StoredProcedure);
        return departments.Find(d => d.IdDepartamento == departmentId);
    }
}