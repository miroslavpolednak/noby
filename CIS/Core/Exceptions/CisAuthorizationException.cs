namespace CIS.Core.Exceptions;

public sealed class CisAuthorizationException
    : System.Security.Authentication.AuthenticationException
{
    public string? Username { get; init; }

    public CisAuthorizationException(string? message = null, string? username = null)
        : base(message)
    {
        Username = username;
    }
}