using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Models;

public class Session()
{
    [JsonPropertyName("Username")]
    public string? Usuario { get; set; }
    [JsonPropertyName("Password")]
    public string? Contrasena { get; set; }
    
}

public class SessionRepository
{
    private readonly string _connectionString;

    public SessionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");
    }

    public async Task<Usuario?> Informacion(Session session)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@Usuario", session.Usuario);
        parameters.Add("@Contrasena", session.Contrasena);

        Usuario? user = await connection.QueryFirstOrDefaultAsync<Usuario>("SesionUsuario", parameters, commandTimeout: 120, commandType: CommandType.StoredProcedure);

        return user;
    }
}