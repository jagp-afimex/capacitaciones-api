using System.Data;
using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class SectionsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete
    [HttpPost(Name = "sections/")]
    public async Task<ActionResult> CreateSection(Seccion section)
    {
        Curso? course = await _context.Cursos.FindAsync(section.IdCurso);

        if (course is null)
            return BadRequest();

        if (section.Nombre is null || section.Nombre.Trim().Equals(""))
            return BadRequest();

        section.IdCursoNavigation = course;

        await _context.Secciones.AddAsync(section);
        await _context.SaveChangesAsync();

        SeccionDto newSection = new()
        {
            IdSeccion = section.IdSeccion,
            Nombre = section.Nombre,
            Orden = section.Orden,
            IdCurso = section.IdCurso,
            Videos = [.. section.Videos.Select(v => new VideoDto{
                IdVideo = v.IdVideo,
                Nombre = v.Nombre,
                IdSeccion = v.IdSeccion,
                Referencia = v.Referencia,
                Duracion = v.Duracion
            })]
        };

        return CreatedAtAction(nameof(SectionById), new { sectionId = section.IdSeccion }, newSection);
    }

    [HttpGet(Name = "sections/")]
    public async Task<IEnumerable<SeccionDto>> Sections()
    {
        List<Seccion> sections = await _context.Secciones
            .Include(s => s.Videos)
            .ToListAsync();

        return from section in sections
               select new SeccionDto
               {
                   IdSeccion = section.IdSeccion,
                   Nombre = section.Nombre,
                   Orden = section.Orden,
                   IdCurso = section.IdCurso,
                   Videos = [.. section.Videos.Select(v => new VideoDto{
                        IdVideo = v.IdVideo,
                        Nombre = v.Nombre,
                        IdSeccion = v.IdSeccion,
                        Referencia = v.Referencia,
                        Duracion = v.Duracion
                    })]
               };
    }

    [HttpGet("{sectionId}", Name = "sections/{sectionId}")]
    public async Task<ActionResult<SeccionDto>> SectionById(int sectionId)
    {
        Seccion? section = await _context.Secciones
            .Include(s => s.Videos)
            .Where(s => s.IdSeccion == sectionId)
            .FirstOrDefaultAsync();

        if (section is null)
            return NotFound();

        return new SeccionDto
        {
            IdSeccion = section.IdSeccion,
            Nombre = section.Nombre,
            Orden = section.Orden,
            IdCurso = section.IdCurso,
            Videos = [.. section.Videos.Select(v => new VideoDto{
                IdVideo = v.IdVideo,
                Nombre = v.Nombre,
                IdSeccion = v.IdSeccion,
                Referencia = v.Referencia,
                Duracion = v.Duracion
            })]
        };
    }

    [HttpPut("{sectionId}", Name = "sections/{sectionId}")]
    public async Task<ActionResult> UpdateSection(int sectionId, Seccion section)
    {
        Seccion? storedSection = await _context.Secciones.FindAsync(sectionId);

        Curso? course = await _context.Cursos.FindAsync(section.IdCurso);

        if (course is null)
            return BadRequest();

        if (storedSection is null)
            return NotFound();

        if (section.Nombre is null || section.Nombre.Trim().Equals(""))
            return BadRequest();

        storedSection.Nombre = section.Nombre;
        storedSection.Orden = section.Orden;
        storedSection.IdCurso = section.IdCurso;
        storedSection.Videos = section.Videos;

        _context.Secciones.Update(storedSection);
        await _context.SaveChangesAsync();

        SeccionDto updatedSection = new()
        {
            IdSeccion = storedSection.IdSeccion,
            Nombre = storedSection.Nombre,
            Orden = storedSection.Orden,
            IdCurso = storedSection.IdCurso,
            Videos = [.. section.Videos.Select(v => new VideoDto{
                IdVideo = v.IdVideo,
                Nombre = v.Nombre,
                IdSeccion = v.IdSeccion,
                Referencia = v.Referencia,
                Duracion = v.Duracion
            })]
        };

        return CreatedAtAction(nameof(SectionById), new { sectionId = storedSection.IdSeccion }, updatedSection);
    }

    [HttpDelete("{sectionId}", Name = "sections/{sectionId}")]
    public async Task<ActionResult> DeleteSection(int sectionId)
    {
        Seccion? section = await _context.Secciones.FindAsync(sectionId);

        if (section is null)
            return NotFound();

        _context.Secciones.Remove(section);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}