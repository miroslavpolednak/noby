namespace FOMS.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    /// <summary>
    /// Login uzivatel z xxvvss databaze.
    /// </summary>
    /// <example>990614w</example>
    public string? Login { get; set; }
}
