using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

public class PositionCourseController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete

    public async Task<ActionResult> InsertPositionForCourse(PuestosCurso positionForCourse)
    {
        Puesto position = await new Puesto(_context).PositionById(positionForCourse.KPuesto);
        Curso? course = await _context.Cursos.FindAsync(positionForCourse.IdCurso);

        if (position is null || course is null)
            return BadRequest();

        await _context.PuestosCursos.AddAsync(positionForCourse);

        PuestosCursoDto newPositionCourse = new()
        {
            IdPuestoCurso = positionForCourse.IdPuestoCurso,
            IdCurso = positionForCourse.IdCurso,
            KPuesto = positionForCourse.KPuesto
        };

        return CreatedAtAction(nameof(PositionsCoursesById), new { positionCourseId = positionForCourse.IdPuestoCurso }, positionForCourse);
    }

    public async Task<ActionResult<PuestosCursoDto>> PositionsCoursesById(int positionCourseId)
    {
        PuestosCurso? positionCourse = await _context.PuestosCursos.FindAsync(positionCourseId);

        if (positionCourse is null)
            return NotFound();

        return new PuestosCursoDto
        {
            IdPuestoCurso = positionCourse.IdPuestoCurso,
            IdCurso = positionCourse.IdCurso,
            KPuesto = positionCourse.KPuesto
        };
    }
}