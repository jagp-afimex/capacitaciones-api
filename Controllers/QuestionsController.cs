

using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "questions/")]
    public async Task<ActionResult> CreateQuestion(Pregunta question)
    {
        TiposPregunta? questionType = _context.TiposPreguntas.Find(question.IdTipoPregunta);

        if (questionType is null || questionType.IdTipoPregunta > 2)
            return BadRequest();

        if (question.TextoPregunta is null || question.TextoPregunta.Trim().Equals(""))
            return BadRequest();

        if (question.IdTipoPregunta == 0)
            return BadRequest();

        await _context.Preguntas.AddAsync(question);
        await _context.SaveChangesAsync();

        PreguntaDto newQuestion;

        if (question.IdTipoPregunta == 1)
        {
            newQuestion = new PreguntaAbiertaDto
            {
                IdPregunta = question.IdPregunta,
                TextoPregunta = question.TextoPregunta,
                IdTipoPregunta = question.IdTipoPregunta,
                IdEvaluacion = question.IdEvaluacion,
                RespuestasPregunta = question.RespuestasPregunta
            };
        }
        else
        {
            newQuestion = new PreguntaOpcionesDto
            {
                IdPregunta = question.IdPregunta,
                TextoPregunta = question.TextoPregunta,
                IdTipoPregunta = question.IdTipoPregunta,
                IdEvaluacion = question.IdEvaluacion,
                OpcionesPregunta = question.OpcionesPregunta
            };
        }

        return CreatedAtAction(nameof(QuestionById), new { questionId = newQuestion.IdPregunta }, newQuestion);

    }

    [HttpGet(Name = "questions/")]
    public async Task<ActionResult<List<PreguntaDto>>> Questions()
    {
        List<Pregunta> questions = await _context.Preguntas.ToListAsync();
        List<PreguntaDto> questionsDto = [];

        foreach (Pregunta question in questions)
        {
            if (question.IdTipoPregunta == 1)
            {
                questionsDto.Add(new PreguntaAbiertaDto
                {
                    IdPregunta = question.IdPregunta,
                    TextoPregunta = question.TextoPregunta,
                    IdTipoPregunta = question.IdTipoPregunta,
                    IdEvaluacion = question.IdEvaluacion,
                    RespuestasPregunta = question.RespuestasPregunta
                });
            }
            else
            {
                questionsDto.Add(new PreguntaOpcionesDto
                {
                    IdPregunta = question.IdPregunta,
                    TextoPregunta = question.TextoPregunta,
                    IdTipoPregunta = question.IdTipoPregunta,
                    IdEvaluacion = question.IdEvaluacion,
                    OpcionesPregunta = question.OpcionesPregunta
                });
            }
        }

        return questionsDto;

    }

    [HttpGet("{questionId}", Name = "questions/{questionId}")]
    public async Task<ActionResult<PreguntaDto>> QuestionById(int questionId)
    {
        Pregunta? question = await _context.Preguntas.FindAsync(questionId);

        if (question is null)
            return NotFound();

        return question.IdTipoPregunta == 1
        ? new PreguntaAbiertaDto
        {
            IdPregunta = question.IdPregunta,
            TextoPregunta = question.TextoPregunta,
            IdTipoPregunta = question.IdTipoPregunta,
            IdEvaluacion = question.IdEvaluacion,
            RespuestasPregunta = question.RespuestasPregunta
        }
        : new PreguntaOpcionesDto
        {
            IdPregunta = question.IdPregunta,
            TextoPregunta = question.TextoPregunta,
            IdTipoPregunta = question.IdTipoPregunta,
            IdEvaluacion = question.IdEvaluacion,
            OpcionesPregunta = question.OpcionesPregunta
        }
        ;

    }

    [HttpPut("{questionId}", Name = "questions/{questionId}")]
    public async Task<ActionResult> UpdateQuestion(int questionId, Pregunta question)
    {
        Pregunta? storedQuestion = await _context.Preguntas.FindAsync(questionId);

        if (storedQuestion is null)
            return NotFound();

        if (question.TextoPregunta is null || question.TextoPregunta.Trim().Equals(""))
            return BadRequest();

        if (question.IdTipoPregunta == 0)
            return BadRequest();

        storedQuestion.TextoPregunta = question.TextoPregunta;
        storedQuestion.IdTipoPregunta = question.IdTipoPregunta;
        storedQuestion.IdEvaluacion = question.IdEvaluacion;

        _context.Preguntas.Update(storedQuestion);
        await _context.SaveChangesAsync();

        PreguntaDto updatedQuestion;

        if (question.IdTipoPregunta == 1)
        {
            updatedQuestion = new PreguntaAbiertaDto
            {
                IdPregunta = question.IdPregunta,
                TextoPregunta = question.TextoPregunta,
                IdTipoPregunta = question.IdTipoPregunta,
                IdEvaluacion = question.IdEvaluacion,
                RespuestasPregunta = question.RespuestasPregunta
            };
        }
        else
        {
            updatedQuestion = new PreguntaOpcionesDto
            {
                IdPregunta = question.IdPregunta,
                TextoPregunta = question.TextoPregunta,
                IdTipoPregunta = question.IdTipoPregunta,
                IdEvaluacion = question.IdEvaluacion,
                OpcionesPregunta = question.OpcionesPregunta
            };
        }

        return CreatedAtAction(nameof(QuestionById), new { questionId = updatedQuestion.IdPregunta }, updatedQuestion);

    }

    [HttpDelete("{questionId}", Name = "questions/{questionId}")]
    public async Task<ActionResult> DeleteQuestion(int questionId)
    {
        Pregunta? question = await _context.Preguntas.FindAsync(questionId);

        if (question is null)
            return NotFound();

        _context.Preguntas.Remove(question);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}