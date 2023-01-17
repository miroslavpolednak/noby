namespace NOBY.Infrastructure.ErrorHandling;

/// <summary>
/// Chyba autentizace - anonymni uzivatel
/// </summary>
public sealed class ApiAuthenticationProblemDetail
{
    /// <summary>
    /// URI CAASu kam ma byt presmerovan browser klienta pro provedeni autentizace.
    /// </summary>
    public string RedirectUri { get; set; } = string.Empty;
}
