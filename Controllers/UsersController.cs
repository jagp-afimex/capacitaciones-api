using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [Route("login/")]
    [HttpPost]
    public async Task<ActionResult<Usuario>> Session(Session session)
    {
        Session session1 = new(_context);
        Usuario? user = await session1.Informacion(session);

        if (user is null)
            return NotFound();

        user.Token = "";
        // user.IdTipoUsuario = 3;
        return user;
    }
}