namespace API_CINE.Exceptions;

// ERROR 401 ( NO AUTENTICADO ) : SE LANZA EN EL LOGIN CUANDO EL CORREO O LA CLAVE NO COINCIDEN
public class CredencialesInvalidasException : Exception
{
    public CredencialesInvalidasException() : base("CORREO O CLAVE INCORRECTOS") { }
}
