namespace NOBY.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    /// <summary>
    /// Login uzivatel do CAASu.
    /// </summary>
    /// <example>KBUID=A09FK3</example>
    public string? Login { get; set; }
}
