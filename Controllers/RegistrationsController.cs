using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

public class RegistrationsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete

    [HttpPost(Name = "registrations/")]
    public async Task<ActionResult> CreateRegistration(Inscripcion registration)
    {
        Curso? course = await _context.Cursos.FindAsync(registration.IdCurso);
        // validar empleado

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
        List<Inscripcion> registrations = await _context.Inscripciones.ToListAsync();

        return from registration in registrations
               select new InscripcionDto
               {
                   IdInscripcion = registration.IdInscripcion,
                   KEmpleado = registration.KEmpleado,
                   IdCurso = registration.IdCurso,
                   Calificacion = registration.Calificacion
               };
    }

    [HttpGet("{registrationId}", Name = "registrations/")]
    public async Task<ActionResult<InscripcionDto>> RegistrationById(int registrationId)
    {
        Inscripcion? registration = await _context.Inscripciones.FindAsync(registrationId);

        if (registration is null)
            return NotFound();

        return new InscripcionDto
        { 
            IdInscripcion = registration.IdInscripcion,
            KEmpleado = registration.KEmpleado,
            IdCurso = registration.IdCurso,
            Calificacion = registration.Calificacion
        };
        
    }

    // [HttpPut("{registrationId}", Name = "registrations/")]
    // public async Task<ActionResult> UpdateRegistration(int registrationId, Inscripcion registration)
    // {

    // }

    // [HttpDelete("{registrationId}", Name = "registrations/")]
}