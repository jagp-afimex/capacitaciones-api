using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("question-options")]
public class QuestionOptionsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;
    

    [HttpPost(Name = "question-options/")]
    public async Task<ActionResult> CreateOption(OpcionesPregunta option)
    {
        if (option.Opcion is null || option.Opcion.Trim().Equals(""))
            return BadRequest();

        if (option.EsRespuesta is null)
            return BadRequest();

        await _context.OpcionesPreguntas.AddAsync(option);
        await _context.SaveChangesAsync();

        OpcionesPreguntaDto newOption = new()
        {
            IdOpcion = option.IdOpcion,
            Opcion = option.Opcion,
            EsRespuesta = option.EsRespuesta,
            IdPregunta = option.IdPregunta
        };

        return CreatedAtAction(nameof(OptionById), new { optionId = option.IdOpcion }, newOption);

    }

    [HttpGet(Name = "question-options/")]
    public async Task<IEnumerable<OpcionesPreguntaDto>> Options()
    {
        List<OpcionesPregunta> options = await _context.OpcionesPreguntas.ToListAsync();

        return from option in options
               select new OpcionesPreguntaDto
               {
                   IdOpcion = option.IdOpcion,
                   Opcion = option.Opcion,
                   EsRespuesta = option.EsRespuesta,
                   IdPregunta = option.IdPregunta
               };
    }

    [HttpGet("{optionId}", Name = "question-options/{optionId}")]
    public async Task<ActionResult<OpcionesPreguntaDto>> OptionById(int optionId)
    {
        OpcionesPregunta? option = await _context.OpcionesPreguntas.FindAsync(optionId);

        if (option is null)
            return NotFound();

        return new OpcionesPreguntaDto
        {
            IdOpcion = option.IdOpcion,
            Opcion = option.Opcion,
            EsRespuesta = option.EsRespuesta
        };
    }

    [HttpPut("{optionId}", Name = "question-options/{optionId}")]
    public async Task<ActionResult> UpdateOption(int optionId, OpcionesPregunta option)
    {
        OpcionesPregunta? storedOption = await _context.OpcionesPreguntas.FindAsync(optionId);

        if (storedOption is null)
            return NotFound();

        if (option.Opcion is null || option.Opcion.Trim().Equals(""))
            return BadRequest();

        if (option.IdPregunta == 0)
            return BadRequest();

        storedOption.Opcion = option.Opcion;
        storedOption.IdPregunta = option.IdPregunta;
        storedOption.EsRespuesta = option.EsRespuesta;

        _context.OpcionesPreguntas.Update(storedOption);
        await _context.SaveChangesAsync();

        OpcionesPreguntaDto updatedOption = new()
        {
            IdOpcion = storedOption.IdOpcion,
            Opcion = storedOption.Opcion,
            EsRespuesta = storedOption.EsRespuesta,
            IdPregunta = storedOption.IdPregunta
        };

        return CreatedAtAction(nameof(OptionById), new { optionId = storedOption.IdOpcion }, updatedOption);
    }

    [HttpDelete("{optionId}", Name = "question-options/{optionId}")]
    public async Task<ActionResult> DeleteOption(int optionId)
    {
        OpcionesPregunta? option = await _context.OpcionesPreguntas.FindAsync(optionId);

        if (option is null)
            return NotFound();

        _context.OpcionesPreguntas.Remove(option);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}