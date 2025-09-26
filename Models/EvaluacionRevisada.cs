using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace capacitaciones_api.Models;

public partial class EvaluacionRevisada
{
    [Key]
    [JsonPropertyName("checkId")]
    public int IdRevision { get; set; }

    [JsonPropertyName("employeeId")]
    public int KEmpleado { get; set; }

    [JsonPropertyName("evaluationId")]
    public int IdEvaluacion { get; set; }

    [JsonPropertyName("date")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("grade")]
    public decimal Calificacion { get; set; }

    [JsonPropertyName("evaluation")]
    public virtual Evaluacion? IdEvaluacionNavigation { get; set; }

    [NotMapped]
    [JsonPropertyName("userEvaluation")]
    public virtual Evaluacion? EvaluacionUsuario { get; set; }

    public static decimal CheckEvaluation(Evaluacion userEvaluation, Evaluacion originalEvaluation)
    {
        int totalQuestions = originalEvaluation.Pregunta.Where(p => p.IdTipoPregunta == 2).Count();
        int correctQuestions = 0;
        decimal finalScore = 0;

        foreach (Pregunta question in userEvaluation.Pregunta.Where(p => p.IdTipoPregunta == 2))
        {
            Pregunta originalQuestion = originalEvaluation.Pregunta.First(p => p.IdPregunta == question.IdPregunta);

            OpcionesPregunta userAnswer = question.OpcionesPregunta.First(p => p.EsRespuesta == true);
            OpcionesPregunta correctAnswer = originalQuestion.OpcionesPregunta.First(p => p.EsRespuesta == true);

            if (userAnswer.IdOpcion == correctAnswer.IdOpcion)
                correctQuestions++;
        }

        finalScore = correctQuestions * 100 / totalQuestions;

        return finalScore;

    }
}

public class EvaluacionRevisadaDto
{
    [JsonPropertyName("checkId")]
    public int IdRevision { get; set; }

    [JsonPropertyName("employeeId")]
    public int KEmpleado { get; set; }

    [JsonPropertyName("evaluationId")]
    public int IdEvaluacion { get; set; }

    [JsonPropertyName("date")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("grade")]
    public decimal Calificacion { get; set; }

    [JsonPropertyName("evaluation")]
    public virtual EvaluacionDto? Evaluacion { get; set; }

    [JsonPropertyName("userEvaluation")]
    public virtual EvaluacionDto? EvaluacionUsuario { get; set; }
    
}