using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;

namespace capacitaciones_api.Models;

public class Credencial
{
    [JsonPropertyName("name")]
    public string Nombre { get; set; }
    [JsonPropertyName("username")]
    public string Usuario { get; set; }
    [JsonPropertyName("password")]
    public string Contrasenia { get; set; }
}

public class CredencialRepository
{
    private readonly string _connectionString;

    public CredencialRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");
    }

    public async Task<List<Credencial>> Credencials()
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Open)
            connection.Open();

        List<Credencial> credencials = (List<Credencial>)await connection.QueryAsync<Credencial>("Credenciales", commandTimeout: 120, commandType: CommandType.StoredProcedure);
        return credencials;
    }
}