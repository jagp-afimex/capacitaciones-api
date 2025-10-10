using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(SessionRepository sessionRepository, CredencialRepository credencialRepository) : ControllerBase
{
    readonly SessionRepository _sessionRepository = sessionRepository;
    readonly CredencialRepository _credencialRepository = credencialRepository;

    [Route("login/")]
    [HttpPost]
    public async Task<ActionResult<Usuario>> Session(Session session)
    {
        Usuario? user = await _sessionRepository.Informacion(session);

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