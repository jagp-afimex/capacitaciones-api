using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("{controller}")]
public class ProgressesController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    // Task<ActionResult> create
    // Task<ActionResult<List<T>>> read
    // Task<ActionResult<T>> readbyid
    // Task<ActionResult> update
    // Task<ActionResult> delete

    [HttpPost(Name = "{controller}/")]
    public async Task<ActionResult> CreateProgress(AvancesCurso progress)
    {
        if (progress.KEmpleado <= 0)
            return BadRequest();

        Estado? state = await _context.Estados.FindAsync(progress.IdEstado);

        if (state is null)
            return BadRequest();

        Curso? course = await _context.Cursos.FindAsync(progress.IdCurso);

        if (course is null)
            return BadRequest();

        Seccion? section = await _context.Secciones.FindAsync(progress.IdSeccion);

        if (section is null || section.IdCurso != course.IdCurso)
            return BadRequest();

        Video? video = await _context.Videos.FindAsync(progress.IdVideo);

        if (video is null || video.IdSeccion != section.IdSeccion)
            return BadRequest();

        progress.Fecha = DateTime.Now;
        progress.VersionCurso = course.Version;

        progress.IdCursoNavigation = course;
        progress.IdVideoNavigation = video;
        progress.IdEstadoNavigation = state;
        progress.IdSeccionNavigation = section;

        await _context.AvancesCursos.AddAsync(progress);
        await _context.SaveChangesAsync();

        AvancesCursoDto newProgress = new()
        {
            IdAvance = progress.IdAvance,
            IdCurso = progress.IdCurso,
            IdSeccion = progress.IdSeccion,
            IdVideo = progress.IdVideo,
            IdEstado = progress.IdEstado,
            Fecha = progress.Fecha,
            VersionCurso = progress.VersionCurso,
            KEmpleado = progress.KEmpleado
        };

        return CreatedAtAction(nameof(ProgressById), new { progressId = progress.IdAvance }, newProgress);

    }

    [HttpGet(Name = "{controller}/")]
    public async Task<IEnumerable<AvancesCursoDto>> Progresses()
    {
        List<AvancesCurso> progresses = await _context.AvancesCursos
            .Include(a => a.IdVideoNavigation)
            .ToListAsync();
        // List<AvancesCurso> progresses = await _context.AvancesCursos.ToListAsync();

        return from progress in progresses
               select new AvancesCursoDto
               {
                   IdAvance = progress.IdAvance,
                   IdCurso = progress.IdCurso,
                   IdSeccion = progress.IdSeccion,
                   IdVideo = progress.IdVideo,
                   IdEstado = progress.IdEstado,
                   Fecha = progress.Fecha,
                   VersionCurso = progress.VersionCurso,
                   KEmpleado = progress.KEmpleado,
                   Orden = progress.IdVideoNavigation.Orden
               };

    }

    [HttpGet("{progressId}", Name = "{controller}/{progressId}")]
    public async Task<ActionResult<AvancesCursoDto>> ProgressById(int progressId)
    {
        AvancesCurso? progress = await _context.AvancesCursos.FindAsync(progressId);

        if (progress is null)
            return NotFound();

        return new AvancesCursoDto
        {
            IdAvance = progress.IdAvance,
            IdCurso = progress.IdCurso,
            IdSeccion = progress.IdSeccion,
            IdVideo = progress.IdVideo,
            IdEstado = progress.IdEstado,
            Fecha = progress.Fecha,
            VersionCurso = progress.VersionCurso,
            KEmpleado = progress.KEmpleado
        };
    }

    // [HttpPut("{progressId}", Name = "{controller}/{progressId}")]

    [HttpDelete("{progressId}", Name = "{controller}/{progressId}")]
    public async Task<ActionResult> DeleteProgress(int progressId)
    {
        AvancesCurso? progress = await _context.AvancesCursos.FindAsync(progressId);

        if (progress is null)
            return NotFound();

        _context.AvancesCursos.Remove(progress);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}