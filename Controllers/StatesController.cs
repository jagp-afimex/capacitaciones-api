
using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatesController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "states/")]
    public async Task<ActionResult> CreateState(Estado state)
    {
        if (state.Nombre.Trim().Equals("") || state.Nombre is null)
            return BadRequest();

        await _context.Estados.AddAsync(state);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(StateById), new { stateId = state.IdEstado }, state);
    }

    [HttpGet(Name = "states/")]
    public async Task<ActionResult<List<Estado>>> States() => new(await _context.Estados.ToListAsync());

    [HttpGet("{stateId}", Name = "/{stateId}")]
    public async Task<ActionResult<Estado>> StateById(int stateId)
    {
        Estado? state = await _context.Estados.FindAsync(stateId);

        return state is null ? NotFound() : state;
    }

    [HttpPut("{stateId}", Name = "states/{stateId}")]    
    public async Task<ActionResult> UpdateState(int stateId, Estado state)
    {
        Estado? storedState = await _context.Estados.FindAsync(stateId);

        if (storedState is null)
            return NotFound();

        if (state.Nombre.Trim().Equals("") || state.Nombre is null)
            return BadRequest();

        storedState.Nombre = state.Nombre;

        _context.Estados.Update(storedState);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(StateById), new { stateId = storedState.IdEstado }, storedState);
    }

    [HttpDelete("{stateId}", Name = "states/{stateId}")]
    public async Task<ActionResult> DeleteState(int stateId)
    {
        Estado? state = await _context.Estados.FindAsync(stateId);

        if (state is null)
            return NotFound();

        _context.Estados.Remove(state);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}