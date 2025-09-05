
using capacitaciones_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VideosController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "videos/")]
    public async Task<ActionResult> CreateVideo(Video video)
    {
        Seccion? section = await _context.Secciones.FindAsync(video.IdSeccion);

        if (section is null)
            return BadRequest();

        if (video.Nombre is null || video.Nombre.Trim().Equals(""))
            return BadRequest();

        if (video.Referencia is null || video.Referencia.Trim().Equals(""))
            return BadRequest();

        // sacarlo programaticamente
        video.Duracion = 10;
        video.IdSeccionNavigation = section;

        await _context.Videos.AddAsync(video);
        await _context.SaveChangesAsync();

        VideoDto newVideo = new()
        {
            IdVideo = video.IdVideo,
            Nombre = video.Nombre,
            IdSeccion = video.IdSeccion,
            Referencia = video.Referencia,
            Duracion = video.Duracion
        };

        return CreatedAtAction(nameof(VideoById), new { videoId = video.IdVideo }, newVideo);
    }

    [HttpGet(Name = "videos/")]
    public async Task<IEnumerable<VideoDto>> Videos()
    {
        List<Video> videos = await _context.Videos.ToListAsync();

        return from video in videos
               select new VideoDto
               {
                   IdVideo = video.IdVideo,
                   Nombre = video.Nombre,
                   IdSeccion = video.IdSeccion,
                   Referencia = video.Referencia,
                   Duracion = video.Duracion
               };

    }

    [HttpGet("{videoId}", Name = "/{videoId}")]
    public async Task<ActionResult<VideoDto>> VideoById(int videoId)
    {
        Video? video = await _context.Videos.FindAsync(videoId);

        if (video is null)
            return NotFound();

        return new VideoDto
        {
            IdVideo = video.IdVideo,
            Nombre = video.Nombre,
            IdSeccion = video.IdSeccion,
            Referencia = video.Referencia,
            Duracion = video.Duracion
        };
    }

    [HttpPut("{videoId}", Name = "videos/{videoId}")]    
    public async Task<ActionResult> UpdateState(int videoId, Video video)
    {
        Video? storedVideo = await _context.Videos.FindAsync(videoId);

        Seccion? section = await _context.Secciones.FindAsync(video.IdSeccion);

        if (section is null)
            return BadRequest();

        if (storedVideo is null)
            return NotFound();

        if (video.Nombre is null || video.Nombre.Trim().Equals(""))
            return BadRequest();

        if (video.Referencia is null || video.Referencia.Trim().Equals(""))
            return BadRequest();

        storedVideo.Nombre = video.Nombre;
        storedVideo.Referencia = video.Referencia;
        storedVideo.Duracion = 60; // sacar programaticamente

        _context.Videos.Update(storedVideo);
        await _context.SaveChangesAsync();

        VideoDto updatedVideo = new()
        { 
            IdVideo = video.IdVideo,
            Nombre = video.Nombre,
            IdSeccion = video.IdSeccion,
            Referencia = video.Referencia,
            Duracion = video.Duracion
        };

        return CreatedAtAction(nameof(VideoById), new { videoId = storedVideo.IdVideo }, updatedVideo);
    }

    [HttpDelete("{videoId}", Name = "videos/{videoId}")]
    public async Task<ActionResult> DeleteState(int videoId)
    {
        Video? video = await _context.Videos.FindAsync(videoId);

        if (video is null)
            return NotFound();

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}