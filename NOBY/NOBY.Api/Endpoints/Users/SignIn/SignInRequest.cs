namespace NOBY.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    /// <summary>
    /// ID uzivatele v ramci vybraneho identitniho schematu.
    /// </summary>
    /// <example>3255</example>
    public string? IdentityId { get; set; }

    /// <summary>
    /// Vybranne identitni schema pro prihlaseni, moznosti jsou: <a href="https://wiki.kb.cz/display/HT/IdentityScheme">https://wiki.kb.cz/display/HT/IdentityScheme</a>
    /// </summary>
    /// <example>M17ID</example>
    public string? IdentityScheme { get; set; }
}
