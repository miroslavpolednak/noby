namespace NOBY.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    /// <summary>
    /// Login uzivatel do CAASu.
    /// </summary>
    /// <example>990614w</example>
    public string? Login { get; set; }
}
