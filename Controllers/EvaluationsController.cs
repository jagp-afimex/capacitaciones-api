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
        List<Evaluacion> evaluations = await _context.Evaluaciones
        .Include(e => e.IdSeccionNavigation)
        //     .ThenInclude(seccion => seccion.IdCursoNavigation)
        .Include(e => e.Pregunta)
            .ThenInclude(opcion => opcion.OpcionesPregunta)
        .Include(e => e.Pregunta)
            .ThenInclude(respuesta => respuesta.RespuestasPregunta)
        .ToListAsync();

        return from evaluation in evaluations
               select new EvaluacionDto
               {
                   IdEvaluacion = evaluation.IdEvaluacion,
                   IdSeccion = evaluation.IdSeccion,
                   Pregunta = [.. evaluation.Pregunta.Select(p => new PreguntaDto
                   {
                        IdPregunta = p.IdPregunta,
                        TextoPregunta = p.TextoPregunta,
                        IdTipoPregunta = p.IdTipoPregunta,
                        IdEvaluacion = p.IdEvaluacion,
                        OpcionesPregunta = [.. p.OpcionesPregunta.Select(o => new OpcionesPreguntaDto{
                            IdOpcion = o.IdOpcion,
                            Opcion = o.Opcion,
                            EsRespuesta = o.EsRespuesta,
                            IdPregunta = o.IdPregunta
                        })],
                        RespuestasPregunta = [.. p.RespuestasPregunta.Select(r => new RespuestasPreguntaDto{
                            IdRespuesta = r.IdRespuesta,
                            Respuesta = r.Respuesta,
                            KEmpleado = r.KEmpleado,
                            IdPregunta = r.IdPregunta
                        })]
                   })],
                   Seccion = new SeccionDto
                   {
                       IdSeccion = evaluation.IdSeccionNavigation.IdSeccion,
                       Nombre = evaluation.IdSeccionNavigation.Nombre
                   },
                   //    Curso = new CursoDto
                   //    {
                   //         IdCurso = evaluation.IdSeccionNavigation.IdCursoNavigation.IdCurso,
                   //         Nombre = evaluation.IdSeccionNavigation.IdCursoNavigation.Nombre,
                   //         Descripcion = evaluation.IdSeccionNavigation.IdCursoNavigation.Descripcion
                   //    }
               };
    }

    [HttpGet("{evaluationId}", Name = "evaluations/{evaluationId}")]
    public async Task<ActionResult<EvaluacionDto>> EvaluationById(int evaluationId)
    {
        Evaluacion? evaluation = await _context.Evaluaciones
        .Include(e => e.IdSeccionNavigation)
        .Include(e => e.Pregunta)
            .ThenInclude(opcion => opcion.OpcionesPregunta)
        .Include(e => e.Pregunta)
            .ThenInclude(respuesta => respuesta.RespuestasPregunta)
        .Where(e => e.IdEvaluacion == evaluationId)
        .FirstOrDefaultAsync();

        if (evaluation is null)
            return NotFound();

        return new EvaluacionDto
        {
            IdEvaluacion = evaluation.IdEvaluacion,
            IdSeccion = evaluation.IdSeccion,
            Pregunta = [.. evaluation.Pregunta.Select(p => new PreguntaDto
            {
                IdPregunta = p.IdPregunta,
                TextoPregunta = p.TextoPregunta,
                IdTipoPregunta = p.IdTipoPregunta,
                IdEvaluacion = p.IdEvaluacion,
                OpcionesPregunta = [.. p.OpcionesPregunta.Select(o => new OpcionesPreguntaDto{
                    IdOpcion = o.IdOpcion,
                    Opcion = o.Opcion,
                    EsRespuesta = o.EsRespuesta,
                    IdPregunta = o.IdPregunta
                })],
                RespuestasPregunta = [.. p.RespuestasPregunta.Select(r => new RespuestasPreguntaDto{
                    IdRespuesta = r.IdRespuesta,
                    Respuesta = r.Respuesta,
                    KEmpleado = r.KEmpleado,
                    IdPregunta = r.IdPregunta
                })]
            })],
            Seccion = new SeccionDto
            {
                IdSeccion = evaluation.IdSeccionNavigation.IdSeccion,
                Nombre = evaluation.IdSeccionNavigation.Nombre,
                Orden = evaluation.IdSeccionNavigation.Orden,
                IdCurso = evaluation.IdSeccionNavigation.IdCurso
            },
        };
    }

    [HttpPut("{evaluationId}", Name = "evaluations/{evaluationId}")]
    public async Task<ActionResult> UpdateEvaluation(int evaluationId, Evaluacion evaluation)
    {
        Evaluacion? storedEvaluation = await _context.Evaluaciones
        .Include(e => e.IdSeccionNavigation)
        .Include(e => e.Pregunta)
            .ThenInclude(opcion => opcion.OpcionesPregunta)
        .Include(e => e.Pregunta)
            .ThenInclude(respuesta => respuesta.RespuestasPregunta)
        .Where(e => e.IdEvaluacion == evaluationId)
        .FirstOrDefaultAsync();

        Seccion? seccion = await _context.Secciones.FindAsync(evaluation.IdSeccion);

        if (seccion is null)
            return BadRequest();

        if (storedEvaluation is null)
            return NotFound();

        storedEvaluation.IdSeccion = evaluation.IdSeccion;

        // ids Pregunta in updateEvaluation
        List<int> idsQuestions = [.. evaluation.Pregunta.Select(p => p.IdPregunta)];
        // find the Preguntas to remove
        List<Pregunta> questionsToRemove = [.. storedEvaluation.Pregunta.Where(p => !idsQuestions.Exists(id => id == p.IdPregunta))];

        foreach (Pregunta question in questionsToRemove)
        {
            storedEvaluation.Pregunta.Remove(question);
        }

        // new or updated question
        foreach (Pregunta question in evaluation.Pregunta)
        {
            Pregunta? existingQuestion = storedEvaluation.Pregunta
            .FirstOrDefault(p => p.IdPregunta == question.IdPregunta);

            if (existingQuestion is not null)
            {
                _context.Entry(existingQuestion).CurrentValues.SetValues(question);

                // las demas entidades relacionadas
                SyncOpcionesPregunta(existingQuestion, question);

                SyncAnswers(existingQuestion, question);
            }
            else
            {
                storedEvaluation.Pregunta.Add(question);
            }
        }

        storedEvaluation.IdSeccionNavigation = seccion;
        // _context.Evaluaciones.Update(storedEvaluation);
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

    private void SyncOpcionesPregunta(Pregunta currentQuestion, Pregunta newQuestion)
    {
        // ids opciones in newQuestion
        List<int> idsOptions = [.. newQuestion.OpcionesPregunta.Select(o => o.IdOpcion)];
        // find the OpcionesPregunta to remove
        List<OpcionesPregunta> optionsToRemove = [.. currentQuestion.OpcionesPregunta.Where(o => !idsOptions.Contains(o.IdOpcion))];

        foreach (OpcionesPregunta option in optionsToRemove)
        {
            currentQuestion.OpcionesPregunta.Remove(option);
        }

        // new or updated oprion
        foreach (OpcionesPregunta option in newQuestion.OpcionesPregunta)
        {
            OpcionesPregunta? existingOption = currentQuestion.OpcionesPregunta
            .FirstOrDefault(o => o.IdOpcion == option.IdOpcion);

            if (existingOption is not null)
            {
                _context.Entry(existingOption).CurrentValues.SetValues(option);
            }
            else
            {
                currentQuestion.OpcionesPregunta.Add(option);
            }
        }
    }

    private void SyncAnswers(Pregunta currentQuestion, Pregunta newQuestion)
    {
        // ids answers in newQuestion
        List<int> idsAnswers = [.. newQuestion.RespuestasPregunta.Select(a => a.IdRespuesta)];
        // find the RespuestasPregunta to remove
        List<RespuestasPregunta> answersToRemove = [.. currentQuestion.RespuestasPregunta.Where(r => !idsAnswers.Contains(r.IdRespuesta))];

        foreach (RespuestasPregunta answer in answersToRemove)
        {
            currentQuestion.RespuestasPregunta.Remove(answer);
        }

        // new or updated answer
        foreach (RespuestasPregunta answer in newQuestion.RespuestasPregunta)
        {
            RespuestasPregunta? existingAnswer = currentQuestion.RespuestasPregunta
            .FirstOrDefault(a => a.IdRespuesta == answer.IdRespuesta);

            if (existingAnswer is not null)
            {
                _context.Entry(existingAnswer).CurrentValues.SetValues(answer);
            }
            else
            {
                currentQuestion.RespuestasPregunta.Add(answer);
            }
        }
    }

    [HttpPost("check/")]
    public async Task<ActionResult<EvaluacionRevisadaDto>> CheckEvaluation(EvaluacionRevisada evaluation)
    {
        Evaluacion? originalEvaluation = await _context.Evaluaciones
        .Include(e => e.IdSeccionNavigation)
        .Include(e => e.Pregunta)
            .ThenInclude(opcion => opcion.OpcionesPregunta)
        .Include(e => e.Pregunta)
            .ThenInclude(respuesta => respuesta.RespuestasPregunta)
        .Where(e => e.IdEvaluacion == evaluation.IdEvaluacion)
        .FirstOrDefaultAsync();

        if (originalEvaluation is null)
            return NotFound();

        decimal finalScore = EvaluacionRevisada.CheckEvaluation(evaluation.IdEvaluacionNavigation, originalEvaluation);

        EvaluacionRevisada revisada = new()
        {
            IdRevision = evaluation.IdRevision,
            KEmpleado = evaluation.KEmpleado,
            IdEvaluacion = evaluation.IdEvaluacion,
            Fecha = DateTime.Now,
            Calificacion = finalScore
        };

        foreach (Pregunta pregunta in evaluation.IdEvaluacionNavigation.Pregunta.Where(p => p.IdTipoPregunta == 1))
        {
            foreach (RespuestasPregunta respuesta in pregunta.RespuestasPregunta)
                await _context.RespuestasPreguntas.AddAsync(respuesta);
        }

        await _context.EvaluacionesRevisadas.AddAsync(revisada);

        await _context.SaveChangesAsync();

        EvaluacionRevisadaDto evaluacionRevisada = new()
        {
            IdRevision = revisada.IdRevision,
            KEmpleado = evaluation.KEmpleado,
            IdEvaluacion = evaluation.IdEvaluacion,
            Fecha = DateTime.Now,
            Calificacion = finalScore,
            Evaluacion = new EvaluacionDto
            {
                IdEvaluacion = originalEvaluation.IdEvaluacion,
                IdSeccion = originalEvaluation.IdSeccion,
                Pregunta = [.. originalEvaluation.Pregunta.Select(p => new PreguntaDto
                {
                    IdPregunta = p.IdPregunta,
                    TextoPregunta = p.TextoPregunta,
                    IdTipoPregunta = p.IdTipoPregunta,
                    IdEvaluacion = p.IdEvaluacion,
                    OpcionesPregunta = [.. p.OpcionesPregunta.Select(o => new OpcionesPreguntaDto{
                        IdOpcion = o.IdOpcion,
                        Opcion = o.Opcion,
                        EsRespuesta = o.EsRespuesta,
                        IdPregunta = o.IdPregunta
                    })],
                    RespuestasPregunta = [.. p.RespuestasPregunta.Select(r => new RespuestasPreguntaDto{
                        IdRespuesta = r.IdRespuesta,
                        Respuesta = r.Respuesta,
                        KEmpleado = r.KEmpleado,
                        IdPregunta = r.IdPregunta
                    })]
                })],
                Seccion = new SeccionDto
                {
                    IdSeccion = originalEvaluation.IdSeccionNavigation.IdSeccion,
                    Nombre = originalEvaluation.IdSeccionNavigation.Nombre
                },
            },
            EvaluacionUsuario = new EvaluacionDto
            {
                IdEvaluacion = evaluation.IdEvaluacionNavigation.IdEvaluacion,
                IdSeccion = evaluation.IdEvaluacionNavigation.IdSeccion,
                Pregunta = [.. evaluation.IdEvaluacionNavigation.Pregunta.Select(p => new PreguntaDto
                {
                    IdPregunta = p.IdPregunta,
                    TextoPregunta = p.TextoPregunta,
                    IdTipoPregunta = p.IdTipoPregunta,
                    IdEvaluacion = p.IdEvaluacion,
                    OpcionesPregunta = [.. p.OpcionesPregunta.Select(o => new OpcionesPreguntaDto{
                        IdOpcion = o.IdOpcion,
                        Opcion = o.Opcion,
                        EsRespuesta = o.EsRespuesta,
                        IdPregunta = o.IdPregunta
                    })],
                    RespuestasPregunta = [.. p.RespuestasPregunta.Select(r => new RespuestasPreguntaDto{
                        IdRespuesta = r.IdRespuesta,
                        Respuesta = r.Respuesta,
                        KEmpleado = r.KEmpleado,
                        IdPregunta = r.IdPregunta
                    })]
                })],
            },
        };

        return evaluacionRevisada;

    }

    [HttpGet("grades/{employeeId}")]
    public async Task<IEnumerable<EvaluacionRevisadaDto>> Grades(int employeeId)
    {
        List<EvaluacionRevisada> grades = await _context.EvaluacionesRevisadas
        .Where(e => e.KEmpleado == employeeId)
        .ToListAsync();

        return [..grades.Select(g => new EvaluacionRevisadaDto{
            IdRevision = g.IdRevision,
            IdEvaluacion = g.IdEvaluacion,
            KEmpleado = g.KEmpleado,
            Fecha = g.Fecha,
            Calificacion = g.Calificacion
        })];
    }

}