
using capacitaciones_api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    readonly CapacitacionesPruebasContext _context;
    readonly InscripcionRepository _inscripcionRepository;

    public CoursesController(CapacitacionesPruebasContext context, InscripcionRepository inscripcionRepository)
    {
        _context = context;
        _inscripcionRepository = inscripcionRepository;
    }

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

        if (course.Descripcion is null || course.Descripcion.Trim().Equals(""))
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
            Descripcion = course.Descripcion,
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
                // Videos = s.Videos
            })]
        };

        return CreatedAtAction(nameof(CourseById), new { courseId = course.IdCurso }, newCourse);
    }

    [HttpGet(Name = "courses/")]
    public async Task<IEnumerable<CursoDto>> Courses()
    {
        List<Curso> storedCourses = await _context.Cursos
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .ToListAsync();

        List<CursoDto> coursesDto = [];

        foreach (Curso course in storedCourses)
        {
            ICollection<InscripcionDto>? registrations = await _inscripcionRepository.Inscripciones(course.IdCurso);

            coursesDto.Add(new CursoDto
            {
                IdCurso = course.IdCurso,
                Nombre = course.Nombre,
                Descripcion = course.Descripcion,
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
                    Videos = [.. s.Videos.Select(v => new VideoDto{
                        IdVideo = v.IdVideo,
                        Nombre = v.Nombre,
                        IdSeccion = v.IdSeccion,
                        Referencia = v.Referencia,
                        Duracion = v.Duracion
                    }) ]
                })],
                Inscripciones = registrations,
            });
        }

        return coursesDto;
    }

    [HttpGet("{courseId}", Name = "courses/{courseId}")]
    public async Task<ActionResult<CursoDto>> CourseById(int courseId)
    {
        Curso? course = await _context.Cursos
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .Where(c => c.IdCurso == courseId).FirstOrDefaultAsync();

        if (course is null)
            return NotFound();

        ICollection<InscripcionDto>? registrations = await _inscripcionRepository.Inscripciones(course.IdCurso);

        return new CursoDto
        {
            IdCurso = course.IdCurso,
            Nombre = course.Nombre,
            Descripcion = course.Descripcion,
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
                Videos = [.. s.Videos.Select(v => new VideoDto{
                    IdVideo = v.IdVideo,
                    Nombre = v.Nombre,
                    IdSeccion = v.IdSeccion,
                    Referencia = v.Referencia,
                    Duracion = v.Duracion
                })]
            })],
            Inscripciones = registrations

        };
    }

    [HttpGet("positions/{positionId}")]
    public async Task<IEnumerable<CursoDto>> CoursesByPosition(int positionId)
    {
        List<Curso> courses = await _context.Cursos
            .Include(c => c.PuestosCursos)
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .ToListAsync();

        courses = [.. courses.Where(c => c.PuestosCursos.AsList().Exists(p => p.KPuesto == positionId))];

        return [.. courses.Select(course => new CursoDto{
            IdCurso = course.IdCurso,
            Nombre = course.Nombre,
            Descripcion = course.Descripcion,
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
                Videos = [.. s.Videos.Select(v => new VideoDto{
                    IdVideo = v.IdVideo,
                    Nombre = v.Nombre,
                    IdSeccion = v.IdSeccion,
                    Referencia = v.Referencia,
                    Duracion = v.Duracion
                })]
            })]
        })];
    }

    [HttpGet("{courseId}/resume/{employeeId}")]
    public async Task<ActionResult<CursoDto>> ResumeCourse(int courseId, int employeeId)
    {
        Curso? course = await _context.Cursos
            .Include(c => c.PuestosCursos)
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.AvancesCursos)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .Where(c => c.IdCurso == courseId).FirstOrDefaultAsync();

        if (course is null)
            return NotFound();

        course.AvancesCursos = [.. course.AvancesCursos.Where(a => a.IdCurso == courseId && a.KEmpleado == employeeId)];

    }
    
}