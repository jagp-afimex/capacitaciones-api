using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public class RespuestasPreguntaOpcion
{
    [Key]
    [JsonPropertyName("answerId")]
    public int IdRespuesta { get; set; }

    [JsonPropertyName("questionId")]
    public int IdPregunta { get; set; }

    [JsonPropertyName("optionId")]
    public int IdOpcionElegida { get; set; }

    [ForeignKey(nameof(EvaluacionRevisada))]
    [JsonPropertyName("checkId")]
    public int IdRevision { get; set; }

    public virtual EvaluacionRevisada? IdEvaluacionRevisadaNavigation { get; set; }

}