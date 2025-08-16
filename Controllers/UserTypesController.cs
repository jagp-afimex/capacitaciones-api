using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("user-types")]
public class UserTypesController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpGet(Name = "user-types/")]
    public async Task<ActionResult<List<TiposUsuario>>> UserTypes()
        => await _context.TiposUsuarios.ToListAsync();

}