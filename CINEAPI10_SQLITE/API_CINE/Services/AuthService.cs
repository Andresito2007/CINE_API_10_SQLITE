using Microsoft.AspNetCore.Identity;
using API_CINE.Dtos;
using API_CINE.Exceptions;

namespace API_CINE.Services;

// NUESTRA LOGICA DE AUTENTICACION
public class ServicioAutenticacion(UserManager<IdentityUser> gestorUsuarios, ServicioToken servicioToken)
{
    public const string ROL_POR_DEFECTO = "usuario";

    // REGISTRA UN USUARIO NUEVO
    public async Task<UsuarioResponse> RegistrarAsync(RegistroRequest request)
    {
        IdentityUser? existente = await gestorUsuarios.FindByEmailAsync(request.Correo);
        if (existente != null)
        {
            throw new ConflictoException("YA EXISTE UN USUARIO CON ESE CORREO");
        }

        // CreateAsync RECIBE LA CLAVE EN TEXTO PERO SOLO GUARDA SU HASH ( AspNetUsers.PasswordHash )
        IdentityUser usuario = new() { UserName = request.Correo, Email = request.Correo };
        IdentityResult resultado = await gestorUsuarios.CreateAsync(usuario, request.Clave);

        if (!resultado.Succeeded)
        {
            string errores = string.Join(" ", resultado.Errors.Select(e => e.Description));
            throw new ReglaNegocioException(errores);
        }

        // TODO USUARIO REGISTRADO NACE CON EL ROL "usuario"
        await gestorUsuarios.AddToRoleAsync(usuario, ROL_POR_DEFECTO);

        return new(usuario.Id, usuario.Email!);
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        IdentityUser? usuario = await gestorUsuarios.FindByEmailAsync(request.Correo);
        if (usuario == null)
        {
            throw new CredencialesInvalidasException();
        }

        // CheckPasswordAsync HASHEA LA CLAVE QUE LLEGO Y COMPARA LOS DOS HASHES
        bool claveCorrecta = await gestorUsuarios.CheckPasswordAsync(usuario, request.Clave);
        if (!claveCorrecta)
        {
            throw new CredencialesInvalidasException();
        }

        // LOS ROLES SE LEEN DE LA BASE Y VIAJAN EN EL TOKEN
        IList<string> roles = await gestorUsuarios.GetRolesAsync(usuario);
        return servicioToken.Generar(usuario, roles);
    }
}
