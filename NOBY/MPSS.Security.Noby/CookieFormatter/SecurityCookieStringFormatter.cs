namespace MPSS.Security.Noby;

/// <summary>
/// Trida zajistujici serializaci a deserializace objektu SecurityCookie.
/// </summary>
internal class SecurityCookieStringFormatter : ISecurityCookieFormatter
{
    /// <summary>
    /// Globalni konfigurace pro soucasnou instanci tridy
    /// </summary>
    private Configuration currentConfiguration;

    public SecurityCookieStringFormatter(Configuration configuration)
    {
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
            Log.LoggerFactory.WriteLog(err, "MPSS.Security.Core.SecurityCookieFormatter", "Decode", 2, "SecCookie decryption: " + cookieValue, Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

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
            Log.LoggerFactory.WriteLog(err, "MPSS.Security.Core.SecurityCookieFormatter", "Decode", 2, "SecCookie deserialization 1: " + err.Message, Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);
            Log.LoggerFactory.WriteLog(err, "MPSS.Security.Core.SecurityCookieFormatter", "Decode", 2, "SecCookie deserialization 2: " + cookieValue, Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

            return null;
        }

        // pokud je cookie v poradku
        if (SecurityCookieFactory.Check(cookie, validator, this.currentConfiguration))
        {
            MpssUser user = new MpssUser(cookie.Identity);
            return user;
        }
        else
        {
            return null;
        }
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
