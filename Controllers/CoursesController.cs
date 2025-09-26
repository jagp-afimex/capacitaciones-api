
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
                Videos = [.. s.Videos.Select(v => new VideoDto{
                        IdVideo = v.IdVideo,
                        Nombre = v.Nombre,
                        IdSeccion = v.IdSeccion,
                        Referencia = v.Referencia,
                        Duracion = v.Duracion
                    }) ]
            })],
            PuestosCursos = [..course.PuestosCursos.Select(p => new PuestosCursoDto{
                IdPuestoCurso = p.IdPuestoCurso,
                KPuesto = p.KPuesto,
                IdCurso = p.IdCurso
            })]
        };

        return CreatedAtAction(nameof(CourseById), new { courseId = course.IdCurso }, newCourse);
    }

    [HttpGet(Name = "courses/")]
    public async Task<IEnumerable<CursoDto>> Courses()
    {
        List<Curso> storedCourses = await _context.Cursos
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.PuestosCursos)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Evaluaciones)
                .ThenInclude(evaluacion => evaluacion.Pregunta)
                .ThenInclude(pregunta => pregunta.OpcionesPregunta)
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
                        Duracion = v.Duracion,
                        Orden = v.Orden
                    }) ],
                    Evaluaciones = [.. s.Evaluaciones.Select(e => new EvaluacionDto{
                        IdEvaluacion = e.IdEvaluacion,
                        IdSeccion = e.IdSeccion,
                        Pregunta = [.. e.Pregunta.Select(p => new PreguntaDto{
                            IdPregunta = p.IdPregunta,
                            TextoPregunta = p.TextoPregunta,
                            IdTipoPregunta = p.IdTipoPregunta,
                            IdEvaluacion = p.IdEvaluacion,
                            OpcionesPregunta = [.. p.OpcionesPregunta.Select(op => new OpcionesPreguntaDto{
                                IdOpcion = op.IdOpcion,
                                Opcion = op.Opcion,
                                EsRespuesta = op.EsRespuesta,
                                IdPregunta = op.IdPregunta
                            })],
                        })]
                    })]
                })],
                Inscripciones = registrations,
                PuestosCursos = [..course.PuestosCursos.Select(p => new PuestosCursoDto{
                    IdPuestoCurso = p.IdPuestoCurso,
                    KPuesto = p.KPuesto,
                    IdCurso = p.IdCurso
                })]
            });
        }

        return coursesDto;
    }

    [HttpGet("{courseId}", Name = "courses/{courseId}")]
    public async Task<ActionResult<CursoDto>> CourseById(int courseId)
    {
        Curso? course = await _context.Cursos
            .Include(c => c.IdCategoriaNavigation)
            .Include(c => c.PuestosCursos)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Videos)
            .Include(c => c.Secciones)
                .ThenInclude(seccion => seccion.Evaluaciones)
                .ThenInclude(evaluacion => evaluacion.Pregunta)
                .ThenInclude(pregunta => pregunta.OpcionesPregunta)
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
                    Duracion = v.Duracion,
                    Orden = v.Orden
                })],
                Evaluaciones = [.. s.Evaluaciones.Select(e => new EvaluacionDto{
                    IdEvaluacion = e.IdEvaluacion,
                    IdSeccion = e.IdSeccion,
                    Pregunta = [.. e.Pregunta.Select(p => new PreguntaDto{
                        IdPregunta = p.IdPregunta,
                        TextoPregunta = p.TextoPregunta,
                        IdTipoPregunta = p.IdTipoPregunta,
                        IdEvaluacion = p.IdEvaluacion,
                        OpcionesPregunta = [.. p.OpcionesPregunta.Select(op => new OpcionesPreguntaDto{
                            IdOpcion = op.IdOpcion,
                            Opcion = op.Opcion,
                            EsRespuesta = op.EsRespuesta,
                            IdPregunta = op.IdPregunta
                        })],
                    })]
                })]
            })],
            Inscripciones = registrations,
            PuestosCursos = [..course.PuestosCursos.Select(p => new PuestosCursoDto{
                IdPuestoCurso = p.IdPuestoCurso,
                KPuesto = p.KPuesto,
                IdCurso = p.IdCurso
            })]

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
                    Duracion = v.Duracion,
                    Orden = v.Orden
                })]
            })],
            PuestosCursos = [..course.PuestosCursos.Select(p => new PuestosCursoDto{
                IdPuestoCurso = p.IdPuestoCurso,
                KPuesto = p.KPuesto,
                IdCurso = p.IdCurso
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
                    Duracion = v.Duracion,
                    Orden = v.Orden
                })]
            })]
        };

    }

    [HttpPut("{courseId}", Name = "courses/{courseId}")]
    public async Task<ActionResult> UpdateCourse(int courseId, Curso course)
    {
        Curso? storedCourse = await _context.Cursos
        .Include(c => c.IdCategoriaNavigation)
        .Include(c => c.PuestosCursos)
        .Include(c => c.Secciones)
            .ThenInclude(seccion => seccion.Videos)
        .Include(c => c.Secciones)
            .ThenInclude(seccion => seccion.Evaluaciones)
            .ThenInclude(evaluacion => evaluacion.Pregunta)
            .ThenInclude(pregunta => pregunta.OpcionesPregunta)
        .Where(c => c.IdCurso == courseId)
        .FirstOrDefaultAsync();

        if (storedCourse is null)
            return NotFound();

        Categoria? category = await _context.Categorias.FindAsync(course.IdCategoria);

        if (category is null)
            return BadRequest();

        if (course.Nombre is null || course.Nombre.Trim().Equals(""))
            return BadRequest();

        if (course.PortadaReferencia is null || course.PortadaReferencia.Trim().Equals(""))
            return BadRequest();

        if (course.Descripcion is null || course.Descripcion.Trim().Equals(""))
            return BadRequest();

        storedCourse.Nombre = course.Nombre;
        storedCourse.PortadaReferencia = course.PortadaReferencia;
        storedCourse.Descripcion = course.Descripcion;

        storedCourse.IdCategoriaNavigation = category;

        // secciones in edited course
        List<int> sectionsIds = [.. course.Secciones.Select(s => s.IdSeccion)];
        // secctiones to remove
        List<Seccion> sectionsToRemove = [.. storedCourse.Secciones.Where(s => !sectionsIds.Contains(s.IdSeccion))];

        foreach (Seccion section in sectionsToRemove)
        {
            storedCourse.Secciones.Remove(section);
        }

        // new or updated section
        foreach (Seccion section in course.Secciones)
        {
            Seccion? existingSection = storedCourse.Secciones.FirstOrDefault(s => s.IdSeccion == section.IdSeccion);

            if (existingSection is not null)
            {
                _context.Entry(existingSection).CurrentValues.SetValues(section);

                SyncVideos(existingSection, section);

            }
            else
            {
                storedCourse.Secciones.Add(section);
            }
        }

        // puestos in edited course
        List<int?> positionsIds = [.. course.PuestosCursos.Select(s => s.KPuesto)];

        // puestos to remove
        List<PuestosCurso> positionsToRemove = [.. storedCourse.PuestosCursos.Where(pc => !positionsIds.Contains(pc.KPuesto))];

        foreach (PuestosCurso position in positionsToRemove)
        {
            storedCourse.PuestosCursos.Remove(position);
        }

        foreach (PuestosCurso position in course.PuestosCursos)
        {
            PuestosCurso? existingPosition = storedCourse.PuestosCursos.FirstOrDefault(pc => pc.KPuesto == position.KPuesto);

            if (existingPosition is not null)
            {
                _context.Entry(existingPosition).CurrentValues.SetValues(position);
            }
            else
            {
                storedCourse.PuestosCursos.Add(position);
            }
        }

        await _context.SaveChangesAsync();

        ICollection<InscripcionDto>? registrations = await _inscripcionRepository.Inscripciones(course.IdCurso);

        CursoDto updatedCourse = new()
        {
            IdCurso = storedCourse.IdCurso,
            Nombre = storedCourse.Nombre,
            Descripcion = storedCourse.Descripcion,
            Version = storedCourse.Version,
            FechaModificacion = storedCourse.FechaModificacion,
            IdCategoria = storedCourse.IdCategoria,
            PortadaReferencia = storedCourse.PortadaReferencia,
            Categoria = new CategoriaDto
            {
                IdCategoria = storedCourse.IdCategoriaNavigation.IdCategoria,
                Nombre = storedCourse.IdCategoriaNavigation.Nombre
            },
            Secciones = [.. storedCourse.Secciones.Select(s => new SeccionDto
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
                    Duracion = v.Duracion,
                    Orden = v.Orden
                })],
                Evaluaciones = [.. s.Evaluaciones.Select(e => new EvaluacionDto{
                    IdEvaluacion = e.IdEvaluacion,
                    IdSeccion = e.IdSeccion,
                    Pregunta = [.. e.Pregunta.Select(p => new PreguntaDto{
                        IdPregunta = p.IdPregunta,
                        TextoPregunta = p.TextoPregunta,
                        IdTipoPregunta = p.IdTipoPregunta,
                        IdEvaluacion = p.IdEvaluacion,
                        OpcionesPregunta = [.. p.OpcionesPregunta.Select(op => new OpcionesPreguntaDto{
                            IdOpcion = op.IdOpcion,
                            Opcion = op.Opcion,
                            EsRespuesta = op.EsRespuesta,
                            IdPregunta = op.IdPregunta
                        })],
                    })]
                })]
            })],
            Inscripciones = registrations,
            PuestosCursos = [..storedCourse.PuestosCursos.Select(p => new PuestosCursoDto{
                IdPuestoCurso = p.IdPuestoCurso,
                KPuesto = p.KPuesto,
                IdCurso = p.IdCurso
            })]

        };

        return CreatedAtAction(nameof(CourseById), new { courseId = storedCourse.IdCurso }, updatedCourse);


    }

    private void SyncVideos(Seccion currentSection, Seccion newSection)
    {
        // ids videos in newSection
        List<int> idsVideos = [.. newSection.Videos.Select(v => v.IdVideo)];

        // find the sections to remove
        List<Video> videosToRemove = [.. currentSection.Videos.Where(v => !idsVideos.Contains(v.IdVideo))];

        // remove the videos
        foreach (Video video in videosToRemove)
            currentSection.Videos.Remove(video);

        // add or update the videos
        foreach (Video video in newSection.Videos)
        {
            Video? existingVideo = currentSection.Videos.FirstOrDefault(v => v.IdVideo == video.IdVideo);

            if (existingVideo is not null)
                _context.Entry(existingVideo).CurrentValues.SetValues(video);
            else
                currentSection.Videos.Add(video);
        }
    }

}