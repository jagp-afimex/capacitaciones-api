using capacitaciones_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("answers")]
public class QuestionAnswers(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "answers/")]
    public async Task<ActionResult> CreateAnswer(RespuestasPregunta answer)
    {
        Pregunta? question = await _context.Preguntas.FindAsync(answer.IdPregunta);

        if (question is null || question.IdTipoPregunta == 2) // pregunta de opcion multiple
            return BadRequest();

        if (answer.Respuesta is null || answer.Respuesta.Trim().Equals(""))
            return BadRequest();

        if (answer.KEmpleado == 0)
            return BadRequest();

        await _context.RespuestasPreguntas.AddAsync(answer);
        await _context.SaveChangesAsync();

        RespuestasPreguntaDto newAnswer = new()
        {
            IdRespuesta = answer.IdRespuesta,
            Respuesta = answer.Respuesta,
            KEmpleado = answer.KEmpleado,
            IdPregunta = answer.IdPregunta
        };

        return CreatedAtAction(nameof(AnswerById), new { answerId = answer.IdRespuesta }, newAnswer);
    }

    [HttpGet(Name = "answers/")]
    public async Task<IEnumerable<RespuestasPreguntaDto>> Answers()
    {
        List<RespuestasPregunta> answers = await _context.RespuestasPreguntas.ToListAsync();

        return from answer in answers
               select new RespuestasPreguntaDto
               {
                   IdRespuesta = answer.IdRespuesta,
                   Respuesta = answer.Respuesta,
                   KEmpleado = answer.KEmpleado,
                   IdPregunta = answer.IdPregunta
               };
    }

    [HttpGet("{answerId}", Name = "answers/{answerId}")]
    public async Task<ActionResult<RespuestasPreguntaDto>> AnswerById(int answerId)
    {
        RespuestasPregunta? answer = await _context.RespuestasPreguntas.FindAsync(answerId);

        if (answer is null)
            return NotFound();

        return new RespuestasPreguntaDto()
        {
            IdRespuesta = answer.IdRespuesta,
            Respuesta = answer.Respuesta,
            KEmpleado = answer.KEmpleado,
            IdPregunta = answer.IdPregunta
        };

    }

    [HttpPut("{answerId}", Name = "answers/{answerId}")]
    public async Task<ActionResult> UpdateAnswer(int answerId, RespuestasPregunta answer)
    {
        RespuestasPregunta? storedAnswer = await _context.RespuestasPreguntas.FindAsync(answerId);

        Pregunta? question = await _context.Preguntas.FindAsync(answer.IdPregunta);

        if (question is null || question.IdTipoPregunta == 2) // pregunta de opcion multiple
            return BadRequest();

        if (storedAnswer is null)
            return NotFound();

        if (answer.Respuesta is null || answer.Respuesta.Trim().Equals(""))
            return BadRequest();

        if (answer.KEmpleado == 0)
            return BadRequest();

        storedAnswer.Respuesta = answer.Respuesta;
        storedAnswer.KEmpleado = answer.KEmpleado;
        storedAnswer.IdPregunta = answer.IdPregunta;

        _context.RespuestasPreguntas.Update(storedAnswer);
        await _context.SaveChangesAsync();

        RespuestasPreguntaDto updatedAnswer = new()
        {
            IdRespuesta = storedAnswer.IdRespuesta,
            Respuesta = storedAnswer.Respuesta,
            KEmpleado = storedAnswer.KEmpleado,
            IdPregunta = storedAnswer.IdPregunta
        };

        return CreatedAtAction(nameof(AnswerById), new { answerId = storedAnswer.IdRespuesta }, updatedAnswer);

    }

    [HttpDelete("{answerId}", Name = "answers/{answerId}")]
    public async Task<IActionResult> DeleteAnswer(int answerId)
    {
        RespuestasPregunta? answer = await _context.RespuestasPreguntas.FindAsync(answerId);

        if (answer is null)
            return NotFound();

        _context.RespuestasPreguntas.Remove(answer);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}