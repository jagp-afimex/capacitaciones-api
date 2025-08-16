using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Models;

public class Session()
{
    [JsonPropertyName("Username")]
    public string? Usuario { get; set; }
    [JsonPropertyName("Password")]
    public string? Contrasena { get; set; }

    private readonly CapacitacionesPruebasContext _context;

    public Session(CapacitacionesPruebasContext context) : this()
    {
        _context = context;
    }

    public async Task<Usuario> Informacion(Session session)
    {
        using DbConnection connection = _context.Database.GetDbConnection();

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@Usuario", session.Usuario);
        parameters.Add("@Contrasena", session.Contrasena);
        return await connection.QueryFirstAsync<Usuario>("SesionUsuario", parameters, commandTimeout: 120, commandType: CommandType.StoredProcedure);
    }
}