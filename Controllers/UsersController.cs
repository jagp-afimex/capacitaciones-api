using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "users/")]
    public async Task<ActionResult<Usuario>> Session(Session session)
    {
        Session session1 = new(_context);
        return await session1.Informacion(session);
    }
}