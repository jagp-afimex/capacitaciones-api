using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("question-types")]
public class QuestionTypesController(CapacitacionesPruebasContext context) : ControllerBase
{
    private readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "question-types/")]
    public async Task<IActionResult> CreateTypeQuestion(TiposPregunta type)
    {
        if (type.Tipo is null || type.Tipo.Trim().Equals(""))
            return BadRequest();

        await _context.TiposPreguntas.AddAsync(type);
        await _context.SaveChangesAsync();

        TipoPreguntaDto newType = new()
        {
            IdTipoPregunta = type.IdTipoPregunta,
            Tipo = type.Tipo
        };

        return CreatedAtAction(nameof(QuestionTypeById), new { questionTypeId = newType.IdTipoPregunta }, newType);

    }

    [HttpGet(Name = "question-types/")]
    public async Task<IEnumerable<TipoPreguntaDto>> QuestionTypes()
    {
        List<TiposPregunta> storedQuestionTypes = await _context.TiposPreguntas.ToListAsync();

        return from type in storedQuestionTypes
               select new TipoPreguntaDto
               {
                   IdTipoPregunta = type.IdTipoPregunta,
                   Tipo = type.Tipo
               };
    }

    [HttpGet("{questionTypeId}", Name = "question-types/{questionTypeId}")]
    public async Task<ActionResult<TipoPreguntaDto>> QuestionTypeById(int questionTypeId)
    {
        TiposPregunta? type = await _context.TiposPreguntas.FindAsync(questionTypeId);

        if (type is null)
            return NotFound();

        return new TipoPreguntaDto
        {
            IdTipoPregunta = type.IdTipoPregunta,
            Tipo = type.Tipo
        };
    }

    [HttpPut("{questionTypeId}", Name = "question-types/{questionTypeId}")]
    public async Task<IActionResult> UpdateQuestionType(int questionTypeId, TiposPregunta type)
    {
        TiposPregunta? storedQuestionType = await _context.TiposPreguntas.FindAsync(questionTypeId);

        if (storedQuestionType is null)
            return NotFound();

        if (type.Tipo is null || type.Tipo.Trim().Equals(""))
            return BadRequest();

        storedQuestionType.Tipo = type.Tipo;

        _context.TiposPreguntas.Update(storedQuestionType);
        await _context.SaveChangesAsync();

        TipoPreguntaDto updatedType = new()
        {
            IdTipoPregunta = storedQuestionType.IdTipoPregunta,
            Tipo = storedQuestionType.Tipo
        };

        return CreatedAtAction(nameof(QuestionTypeById), new { questionTypeId = storedQuestionType.IdTipoPregunta }, updatedType);
    }

    [HttpDelete("{questionTypeId}", Name = "question-types/{questionTypeId}")]
    public async Task<ActionResult> DeleteQuestionType(int questionTypeId)
    {
        TiposPregunta? type = await _context.TiposPreguntas.FindAsync(questionTypeId);

        if (type is null)
            return NotFound();

        _context.TiposPreguntas.Remove(type);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}