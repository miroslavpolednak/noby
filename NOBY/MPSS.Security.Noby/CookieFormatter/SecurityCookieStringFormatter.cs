namespace MPSS.Security.Noby;

/// <summary>
/// Trida zajistujici serializaci a deserializace objektu SecurityCookie.
/// </summary>
internal class SecurityCookieStringFormatter : ISecurityCookieFormatter
{
    /// <summary>
    /// Globalni konfigurace pro soucasnou instanci tridy
    /// </summary>
    private readonly Configuration currentConfiguration;
    private readonly ILogger<IPortal> _logger;

    public SecurityCookieStringFormatter(Configuration configuration, ILogger<IPortal> logger)
    {
        _logger = logger;
        this.currentConfiguration = configuration;
    }

    /// <summary>
    /// Serializace a encryptovani security cookie.
    /// </summary>
    /// <param name="identity">Instance uzivatele</param>
    /// <param name="validator">Instance cookie validatoru.</param>
    /// <returns>Vraci string zastupujici serializovany objekt SecurityCookie</returns>
    public string Encode(IdentityBase identity, CookieValidator validator)
    {
        return CryptoFactory.GetCryptoProvider(this.currentConfiguration).Encrypt(SecurityCookieFactory.Get(identity, validator).ToByteArray());
    }

    public MpssUser Decode(string cookieValue, CookieValidator validator, out SecurityCookie cookie)
    {
#pragma warning disable CS8603 // Possible null reference return.
        cookie = null!;
        byte[] dec;
        try
        {
            // dekryptovat retezec z cookie
            dec = CryptoFactory.GetCryptoProvider(this.currentConfiguration).Decrypt(Convert.FromBase64String(cookieValue));
        }
        catch (Exception err) // chyba pri dekodovani
        {
            // logovat mozny utok
            _logger.LogInformation($"SecurityCookieFormatter: decryption: {cookieValue} - {err.Message}");
            return null;
        }

        // deserializace retezce
        try
        {
            cookie = SecurityCookie.GetFromByteArray(dec);
        }
        catch (Exception err) // chyba pri deserializace
        {
            // logovat mozny utok
            _logger.LogInformation(err, "SecurityCookieFormatter: deserialization 1: " + err.Message);
            _logger.LogInformation(err, "SecurityCookieFormatter: deserialization 2: " + cookieValue);

            return null;
        }

        // pokud je cookie v poradku
        MpssUser user = new MpssUser(cookie.Identity);
        return user;
    }

    /// <summary>
    /// Decryptovani a deserializace security cookie.
    /// </summary>
    /// <param name="cookieValue">String obsahujici hodnotu cookie.</param>
    /// <param name="validator">Instance validatoru security cookie.</param>
    /// <returns>Vraci instanci uzivatele. Pokud byla cookie neplatna, vraci instanci neplatneho uzivatele (IsAuthenticated=false)</returns>
    public MpssUser Decode(string cookieValue, CookieValidator validator)
    {
        SecurityCookie cookie;
        return Decode(cookieValue, validator, out cookie);
    }
}
