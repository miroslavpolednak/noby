namespace MPSS.Security.Noby;

/// <summary>
/// Trida pro praci s objektem SecurityCookie. Umoznuje vytvoreni nove SecurityCookie nebo overeni platnosti stavajici SecurityCookie.
/// </summary>
internal class SecurityCookieFactory
{
    /// <summary>
    /// Vraci tridu, ktera zajistuje formatovani security cookie.
    /// </summary>
    /// <returns></returns>
    public static ISecurityCookieFormatter GetFormatter(Configuration c, ILogger<IPortal> logger)
    {
        return new SecurityCookieStringFormatter(c, logger);
    }

    /// <summary>
    /// Vytvari novy objekt SecurityCookie, ktery se serializovany uklada do kolekce cookies.
    /// </summary>
    /// <param name="identity">Instance uzivatele</param>
    /// <param name="validator">Instance cookie validatoru.</param>
    /// <returns>Vraci objekt naplneny udaji pro overeni platnosti a instanci uzivatele</returns>
    public static SecurityCookie Get(IdentityBase identity, CookieValidator validator)
    {
        SecurityCookie cookie = new SecurityCookie();
        cookie.Identity = identity;
        cookie.LastUpdate = DateTime.Now;
        cookie.IP = validator.IP;
        cookie.UserAgent = validator.UserAgent;
        return cookie;
    }
}
