using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using API_CINE.Dtos;

namespace API_CINE.Services;

// Fabrica el JWT ( JSON Web Token ) que el cliente recibe al hacer login.
public class ServicioToken(IConfiguration configuracion)
{
    public TokenResponse Generar(IdentityUser usuario, IList<string> roles)
    {
        //  LOS DATOS QUE VAN DENTRO DEL TOKEN ( CLAIMS )
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, usuario.Id),
            new(ClaimTypes.Name, usuario.UserName!),
            new(ClaimTypes.Email, usuario.Email!),
        ];

        // EL ROL VIAJA DENTRO DEL TOKEN : SIN ESTA LINEA , [Authorize(Roles = ...)] NUNCA PASA
        claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["Jwt:Clave"]!));

        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        // ARMAR EL TOKEN
        DateTime expira = DateTime.UtcNow.AddMinutes(int.Parse(configuracion["Jwt:MinutosExpiracion"]!));
        var token = new JwtSecurityToken(
            issuer: configuracion["Jwt:Emisor"],
            audience: configuracion["Jwt:Audiencia"],
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);
            
        string texto = new JwtSecurityTokenHandler().WriteToken(token);
        return new(texto, expira);
    }
}
