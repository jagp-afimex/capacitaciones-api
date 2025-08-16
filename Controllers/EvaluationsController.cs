using System.Net.Http.Headers;
using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EvaluationsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete

    [HttpPost(Name = "evaluations/")]
    public async Task<ActionResult> CreateEvaluation(Evaluacion evaluation)
    {
        Seccion? section = await _context.Secciones.FindAsync(evaluation.IdSeccion);

        if (section is null)
            return BadRequest();

        evaluation.IdSeccionNavigation = section;

        await _context.Evaluaciones.AddAsync(evaluation);
        await _context.SaveChangesAsync();

        EvaluacionDto newEvaluation = new()
        {
            IdEvaluacion = evaluation.IdEvaluacion,
            IdSeccion = evaluation.IdSeccion
        };

        return CreatedAtAction(nameof(EvaluationById), new { evaluationId = evaluation.IdEvaluacion }, newEvaluation);
    }

    [HttpGet(Name = "evaluations/")]
    public async Task<IEnumerable<EvaluacionDto>> Evaluations()
    {
        List<Evaluacion> evaluations = await _context.Evaluaciones.ToListAsync();

        return from evaluation in evaluations
               select new EvaluacionDto
               {
                   IdEvaluacion = evaluation.IdEvaluacion,
                   IdSeccion = evaluation.IdSeccion
               };
    }

    [HttpGet("{evaluationId}", Name = "evaluations/{evaluationId}")]
    public async Task<ActionResult<EvaluacionDto>> EvaluationById(int evaluationId)
    {
        Evaluacion? evaluation = await _context.Evaluaciones.FindAsync(evaluationId);

        if (evaluation is null)
            return NotFound();

        return new EvaluacionDto
        {
            IdEvaluacion = evaluation.IdEvaluacion,
            IdSeccion = evaluation.IdSeccion
        };
    }

    [HttpPut("{evaluationId}", Name = "evaluations/{evaluationId}")]
    public async Task<ActionResult> UpdateEvaluation(int evaluationId, Evaluacion evaluation)
    {
        Evaluacion? storedEvaluation = await _context.Evaluaciones.FindAsync(evaluationId);

        Seccion? seccion = await _context.Secciones.FindAsync(evaluation.IdSeccion);

        if (seccion is null)
            return BadRequest();

        if (storedEvaluation is null)
            return NotFound();

        storedEvaluation.IdSeccion = evaluation.IdSeccion;

        _context.Evaluaciones.Update(storedEvaluation);
        await _context.SaveChangesAsync();

        EvaluacionDto updatedEvaluation = new()
        {
            IdEvaluacion = storedEvaluation.IdEvaluacion,
            IdSeccion = storedEvaluation.IdSeccion
        };

        return CreatedAtAction(nameof(EvaluationById), new { evaluationId = storedEvaluation.IdEvaluacion }, updatedEvaluation);
    }

    [HttpDelete("{evaluationId}", Name = "evaluations/{evaluationId}")]
    public async Task<ActionResult> DeleteEvaluation(int evaluationId)
    {
        Evaluacion? evaluation = await _context.Evaluaciones.FindAsync(evaluationId);

        if (evaluation is null)
            return NotFound();

        _context.Evaluaciones.Remove(evaluation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}