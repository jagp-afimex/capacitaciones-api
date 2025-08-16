
using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "courses/")]
    public async Task<ActionResult> CreateCourse(Curso course)
    {
        Categoria? category = await _context.Categorias.FindAsync(course.IdCategoria);

        if (category is null)
            return BadRequest();

        if (course.Nombre is null || course.Nombre.Trim().Equals(""))
            return BadRequest();

        if (course.PortadaReferencia is null || course.PortadaReferencia.Trim().Equals(""))
            return BadRequest();

        course.IdCategoria = category.IdCategoria;
        course.Version = 1;
        course.FechaModificacion = DateTime.Now;

        await _context.Cursos.AddAsync(course);
        await _context.SaveChangesAsync();

        CursoDto newCourse = new()
        {
            IdCurso = course.IdCurso,
            Nombre = course.Nombre,
            Version = course.Version,
            FechaModificacion = course.FechaModificacion,
            IdCategoria = course.IdCategoria,
            PortadaReferencia = course.PortadaReferencia,
            Categoria = new CategoriaDto
            {
                IdCategoria = course.IdCategoriaNavigation.IdCategoria,
                Nombre = course.IdCategoriaNavigation.Nombre
            },
            Secciones = [.. course.Secciones.Select(s => new SeccionDto
            {
                IdSeccion = s.IdSeccion,
                Nombre = s.Nombre,
                Orden = s.Orden,
                IdCurso = s.IdCurso,
                Videos = s.Videos
            })]
        };

        return CreatedAtAction(nameof(CourseById), new { courseId = course.IdCurso }, newCourse);
    }

    [HttpGet(Name = "courses/")]
    public async Task<IEnumerable<CursoDto>> Courses()
    {
        List<Curso> storedCourses = await _context.Cursos
            .Include(c => c.Secciones)
            .Include(c => c.IdCategoriaNavigation)
            .ToListAsync();

        List<CursoDto> coursesDto = [];

        foreach (Curso course in storedCourses)
        {
            coursesDto.Add(new CursoDto
            {
                IdCurso = course.IdCurso,
                Nombre = course.Nombre,
                Version = course.Version,
                FechaModificacion = course.FechaModificacion,
                IdCategoria = course.IdCategoria,
                PortadaReferencia = course.PortadaReferencia,
                Categoria = new CategoriaDto
                {
                    IdCategoria = course.IdCategoriaNavigation.IdCategoria,
                    Nombre = course.IdCategoriaNavigation.Nombre
                },
                Secciones = [.. course.Secciones.Select(s => new SeccionDto
                {
                    IdSeccion = s.IdSeccion,
                    Nombre = s.Nombre,
                    Orden = s.Orden,
                    IdCurso = s.IdCurso,
                    Videos = s.Videos
                })]
            });
        }

        return coursesDto;
    }

    [HttpGet("{courseId}", Name = "courses/{courseId}")]
    public async Task<ActionResult<CursoDto>> CourseById(int courseId)
    {
        Curso? course = await _context.Cursos
            .Include(c => c.Secciones)
            .Include(c => c.IdCategoriaNavigation)
            .Where(c => c.IdCurso == courseId).FirstOrDefaultAsync();

        if (course is null)
            return NotFound();

        return new CursoDto
        {
            IdCurso = course.IdCurso,
            Nombre = course.Nombre,
            Version = course.Version,
            FechaModificacion = course.FechaModificacion,
            IdCategoria = course.IdCategoria,
            PortadaReferencia = course.PortadaReferencia,
            Categoria = new CategoriaDto
            {
                IdCategoria = course.IdCategoriaNavigation.IdCategoria,
                Nombre = course.IdCategoriaNavigation.Nombre
            },
            Secciones = [.. course.Secciones.Select(s => new SeccionDto
            {
                IdSeccion = s.IdSeccion,
                Nombre = s.Nombre,
                Orden = s.Orden,
                IdCurso = s.IdCurso,
                Videos = s.Videos
            })]
        };
    }
    
}