namespace NOBY.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    /// <summary>
    /// Login uzivatel do CAASu.
    /// </summary>
    /// <example>614</example>
    public string? Login { get; set; }

    /// <summary>
    /// Identitni schema
    /// </summary>
    /// <example>OsCis</example>
    public string? Schema { get; set; }
}
