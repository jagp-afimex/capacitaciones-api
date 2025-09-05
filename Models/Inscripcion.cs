using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Models;

public partial class Inscripcion
{
    [JsonPropertyName("registrationId")]
    public int IdInscripcion { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("grade")]
    public decimal? Calificacion { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }
}

public class InscripcionDto
{
    [JsonPropertyName("registrationId")]
    public int IdInscripcion { get; set; }

    [JsonPropertyName("courseId")]
    public int? IdCurso { get; set; }

    [JsonPropertyName("employeeId")]
    public int? KEmpleado { get; set; }

    [JsonPropertyName("employeeName")]
    public string? Nombre { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime? FechaInicio { get; set; }

    [JsonPropertyName("finishDate")]
    public DateTime? FechaFin { get; set; }

    [JsonPropertyName("grade")]
    public decimal? Calificacion { get; set; }

    [JsonPropertyName("progress")]
    public decimal? Progreso { get; set; }
}

public class InscripcionRepository
{

    private readonly string _connectionString;

    public InscripcionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CapacitacionesPruebas");
    }

    public async Task<ICollection<InscripcionDto>?> Inscripciones(int idCurso)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@idCurso", idCurso);
        var results = await connection.QueryAsync<InscripcionDto>("Sl_Inscripciones", parameters, null, 300, CommandType.StoredProcedure);

        return results.AsList();
    }

    public async Task<ICollection<InscripcionDto>?> InscripcionesPorEmpleado(int idEmpleado)
    {
        using DbConnection connection = new SqlConnection(_connectionString);

        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@idEmpleado", idEmpleado);
        var results = await connection.QueryAsync<InscripcionDto>("Sl_Inscripciones", parameters, null, 300, CommandType.StoredProcedure);

        return results.AsList();
    }

    
}