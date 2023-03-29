namespace CIS.Core.Exceptions;

public sealed class CisAuthorizationException
    : System.Security.Authentication.AuthenticationException
{
    public CisAuthorizationException(string? message = null)
        : base(message)
    {
    }
}