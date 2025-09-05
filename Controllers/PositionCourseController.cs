using capacitaciones_api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("position-course")]
public class PositionCourseController : ControllerBase
{
    readonly CapacitacionesPruebasContext _context;
    readonly PuestoRepository _puestoRepository;

    public PositionCourseController(CapacitacionesPruebasContext context, PuestoRepository puestoRepository)
    {
        _context = context;
        _puestoRepository = puestoRepository;
    }

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete
    [HttpPost(Name = "position-course/")]
    public async Task<ActionResult> InsertPositionForCourse(PuestosCurso positionForCourse)
    {
        Puesto position = await _puestoRepository.PositionById(positionForCourse.KPuesto);
        Curso? course = await _context.Cursos.FindAsync(positionForCourse.IdCurso);

        if (position is null || course is null)
            return BadRequest();

        await _context.PuestosCursos.AddAsync(positionForCourse);
        await _context.SaveChangesAsync();

        PuestosCursoDto newPositionCourse = new()
        {
            IdPuestoCurso = positionForCourse.IdPuestoCurso,
            IdCurso = positionForCourse.IdCurso,
            KPuesto = positionForCourse.KPuesto
        };

        return CreatedAtAction(nameof(PositionsCoursesById), new { positionCourseId = positionForCourse.IdPuestoCurso }, newPositionCourse);
    }

    [HttpGet("{positionCourseId}", Name = "/{positionCourseId}")]
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