using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_CINE.Dtos;
using API_CINE.Services;

namespace API_CINE.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ServicioAutenticacion servicio) : ControllerBase
{
    [HttpPost("registro")]
    public async Task<ActionResult<UsuarioResponse>> Registrar(RegistroRequest request)
    {
        UsuarioResponse usuario = await servicio.RegistrarAsync(request);
        return Created("/api/auth/yo", usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
    {
        return Ok(await servicio.LoginAsync(request));
    }

    [Authorize]
    [HttpGet("yo")]
    public ActionResult<UsuarioResponse> Yo()
    {
        string id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        string correo = User.FindFirstValue(ClaimTypes.Email)!;
        return Ok(new UsuarioResponse(id, correo));
    }
}
