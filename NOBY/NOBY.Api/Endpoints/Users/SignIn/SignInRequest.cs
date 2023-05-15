namespace NOBY.Api.Endpoints.Users.SignIn;

public sealed class SignInRequest : IRequest
{
    [Obsolete]
    public string? Login { get; set; }

    /// <summary>
    /// ID uzivatele v ramci vybraneho identitniho schematu.
    /// </summary>
    /// <example>E0021L</example>
    public string? IdentityId { get; set; }

    /// <summary>
    /// Vybranne identitni schema pro prihlaseni, moznosti jsou: <a href="https://wiki.kb.cz/display/HT/IdentityScheme">https://wiki.kb.cz/display/HT/IdentityScheme</a>
    /// </summary>
    /// <example>KBUID</example>
    public string? IdentityScheme { get; set; }
}
