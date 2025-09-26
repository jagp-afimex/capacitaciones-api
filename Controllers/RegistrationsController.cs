using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("{controller}")]
public class RegistrationsController : ControllerBase
{
    readonly CapacitacionesPruebasContext _context;
    readonly InscripcionRepository _inscripcionRepository;
    readonly EmpleadoRepository _empleadoRepository;

    public RegistrationsController(CapacitacionesPruebasContext context, InscripcionRepository inscripcionRepository, EmpleadoRepository empleadoRepository)
    {
        _context = context;
        _inscripcionRepository = inscripcionRepository;
        _empleadoRepository = empleadoRepository;
    }

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete

    [HttpPost(Name = "registrations/")]
    public async Task<ActionResult> CreateRegistration(Inscripcion registration)
    {
        List<Inscripcion> registrations = await _context.Inscripciones.ToListAsync();

        Empleado? employee = await _empleadoRepository.EmployeeById(registration.KEmpleado);

        bool registrationExist = registrations.Exists(r => r.IdCurso == registration.IdCurso && r.KEmpleado == registration.KEmpleado);

        if (registrationExist)
            return BadRequest();

        Curso? course = await _context.Cursos.FindAsync(registration.IdCurso);

        // validar empleado
        if (employee is null)
            return BadRequest();

        if (course is null)
            return BadRequest();

        if (registration.KEmpleado == 0)
            return BadRequest();

        if (registration.Calificacion < 0)
            return BadRequest();

        registration.IdCursoNavigation = course;

        await _context.Inscripciones.AddAsync(registration);
        await _context.SaveChangesAsync();

        InscripcionDto newRegistration = new()
        {
            IdInscripcion = registration.IdInscripcion,
            KEmpleado = registration.KEmpleado,
            IdCurso = course.IdCurso,
            Calificacion = registration.Calificacion
        };

        return CreatedAtAction(nameof(RegistrationById), new { registrationId = registration.IdInscripcion }, newRegistration);

    }

    [HttpGet(Name = "registrations/")]
    public async Task<IEnumerable<InscripcionDto>> Registrations()
    {
        // List<Inscripcion> registrations = await _context.Inscripciones.ToListAsync();

        // return from registration in registrations
        //        select new InscripcionDto()
        //        {
        //            IdInscripcion = registration.IdInscripcion,
        //            KEmpleado = registration.KEmpleado,
        //            IdCurso = registration.IdCurso,
        //            Calificacion = registration.Calificacion
        //        };

        List<InscripcionDto> registrations = (List<InscripcionDto>)await _inscripcionRepository.Inscripciones();

        return registrations;
    }

    [HttpGet("{registrationId}", Name = "registrations/{registrationId}")]
    public async Task<ActionResult<InscripcionDto>> RegistrationById(int registrationId)
    {
        Inscripcion? registration = await _context.Inscripciones.FindAsync(registrationId);

        if (registration is null)
            return NotFound();

        return new InscripcionDto()
        {
            IdInscripcion = registration.IdInscripcion,
            KEmpleado = registration.KEmpleado,
            IdCurso = registration.IdCurso,
            Calificacion = registration.Calificacion
        };

    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<CursoDto>>> RegistrationByEmployee(int employeeId)
    {
        // validar empleado
        Empleado? employee = await _empleadoRepository.EmployeeById(employeeId);

        if (employee is null)
            return NotFound();

        List<InscripcionDto> registrations = (List<InscripcionDto>)await _inscripcionRepository.InscripcionesPorEmpleado(employeeId);

        // return registrations;
        List<Curso>? courses = await _context.Cursos
            .Include(c => c.Secciones)
            .Include(c => c.IdCategoriaNavigation)
            .ToListAsync();

        courses = [.. courses.Where(c => registrations.Exists(r => r.IdCurso == c.IdCurso))];

        List<CursoDto> coursesDto = [.. courses.Select(course => new CursoDto{
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
        })];

        coursesDto.ForEach(c => c.Inscripciones = registrations.Where(r => r.IdCurso == c.IdCurso).ToList());

        return coursesDto;
    }

    [HttpPut("{registrationId}", Name = "registrations/{registrationId}")]
    public async Task<ActionResult> UpdateRegistration(int registrationId, Inscripcion registration)
    {
        Inscripcion? storedRegistration = await _context.Inscripciones.FindAsync(registrationId);

        if (storedRegistration is null)
            return NotFound();

        Curso? course = await _context.Cursos
        .Include(c => c.Secciones)
            .ThenInclude(seccion => seccion.Evaluaciones)
        .Where(c => c.IdCurso == registration.IdCurso)
        .FirstOrDefaultAsync();

        if (course is null)
            return BadRequest();

        if (registration.KEmpleado != storedRegistration.KEmpleado)
            return BadRequest();

        List<int> evaluationsIds = [];

        foreach (Seccion section in course.Secciones)
        {
            evaluationsIds.AddRange([.. section.Evaluaciones.Select(e => e.IdEvaluacion)]);
        }

        List<EvaluacionRevisada> revisadas = await _context.EvaluacionesRevisadas
            .Where(e => e.KEmpleado == registration.KEmpleado)
            .ToListAsync();

        revisadas = [.. revisadas.Where(r => evaluationsIds.Contains(r.IdEvaluacion))];

        var gruposPorEvaluacion = revisadas.GroupBy(r => r.IdEvaluacion);

        var calificacionMaximaPorEvaluacion = gruposPorEvaluacion.Select(grupo => new
        {
            IdEvaluacion = grupo.Key,
            CalificacionMaxima = grupo.Max(e => e.Calificacion)
        });

        decimal sumCalificaciones = 0;
        decimal promedio = 0;
        int totalEvaluaciones = 0;

        foreach (var item in calificacionMaximaPorEvaluacion)
        {
            sumCalificaciones += item.CalificacionMaxima;
            totalEvaluaciones++;
        }

        if (totalEvaluaciones > 0)
            promedio = sumCalificaciones / totalEvaluaciones;

        storedRegistration.Calificacion = promedio;
        storedRegistration.IdCurso = course.IdCurso;

        _context.Update(storedRegistration);
        await _context.SaveChangesAsync();

        List<InscripcionDto> employeeRegistrations = (List<InscripcionDto>)await _inscripcionRepository.InscripcionesPorEmpleado((int)storedRegistration.KEmpleado);

        InscripcionDto updatedRegistration = employeeRegistrations.First(r => r.IdCurso == course.IdCurso);

        return CreatedAtAction(nameof(RegistrationById), new { registrationId = storedRegistration.IdInscripcion }, updatedRegistration);

    }

    [HttpDelete("{registrationId}", Name = "registrations/{registrationId}")]
    public async Task<ActionResult> DeleteRegistration(int registrationId)
    {
        Inscripcion? registration = await _context.Inscripciones.FindAsync(registrationId);

        if (registration is null)
            return NotFound();

        _context.Inscripciones.Remove(registration);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}