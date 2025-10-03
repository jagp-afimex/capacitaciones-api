using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(CapacitacionesPruebasContext context, CredencialRepository credencialRepository) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;
    readonly CredencialRepository _credencialRepository = credencialRepository;

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

    [Route("credencials/")]
    [HttpGet]
    public async Task<ActionResult<List<Credencial>>> Credencials()
    {
        List<Credencial> credencials = await _credencialRepository.Credencials();

        return credencials;
    }
}