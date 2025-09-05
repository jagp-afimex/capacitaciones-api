using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionsController(PuestoRepository puestoRepository) : ControllerBase
{
    readonly PuestoRepository _puestoRepository = puestoRepository;

    [HttpGet(Name = "positions/")]
    public async Task<List<Puesto>> Positions()
    {
        List<Puesto> positions = await _puestoRepository.Positions();
        return positions;
    }

    [HttpGet("{positionId}", Name = "positions/{positionId}")]
    public async Task<ActionResult<Puesto>> PositionBy(int positionId)
    {
        Puesto position = await _puestoRepository.PositionById(positionId);
        return position is null ? NotFound() : position;
    }
}