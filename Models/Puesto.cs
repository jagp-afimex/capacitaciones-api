using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Models;

public class Puesto()
{
    [JsonPropertyName("positionId")]
    public int IdPuesto { get; set; }
    [JsonPropertyName("positionName")]
    public string? Nombre { get; set; }

}

public class PuestoRepository
{
    private readonly string _connectionString;

    public PuestoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");
    }

    public async Task<List<Puesto>> Positions()
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Open)
            connection.Open();

        List<Puesto> positions = (List<Puesto>)await connection.QueryAsync<Puesto>("Puestos", commandTimeout: 120, commandType: CommandType.StoredProcedure);
        return positions;
    }

    public async Task<Puesto> PositionById(int? positionId)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Open)
            connection.Open();

        List<Puesto> positions = (List<Puesto>)await connection.QueryAsync<Puesto>("Puestos", commandTimeout: 120, commandType: CommandType.StoredProcedure);
        return positions.Find(p => p.IdPuesto == positionId);
    }
}