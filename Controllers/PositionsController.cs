using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionsController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpGet(Name = "positions/")]
    public async Task<List<Puesto>> Positions()
    {
        Puesto position = new(_context);
        List<Puesto> positions = await position.Positions();
        return positions;
    }

    [HttpGet("{positionId}", Name = "positions/{positionId}")]
    public async Task<ActionResult<Puesto>> PositionBy(int positionId)
    {
        Puesto? position = new(_context);
        position = await position.PositionById(positionId);
        return position is null ? NotFound() : position;
    }
}