namespace MPSS.Security.Noby;

internal interface ISecurityCookieFormatter
{
    /// <summary>
    /// Serializace a encryptovani security cookie.
    /// </summary>
    /// <param name="identity">Instance uzivatele</param>
    /// <param name="validator">Instance cookie validatoru.</param>
    /// <returns>Vraci string zastupujici serializovany objekt SecurityCookie</returns>
    string Encode(IdentityBase identity, CookieValidator validator);

    /// <summary>
    /// Decryptovani a deserializace security cookie.
    /// </summary>
    /// <param name="cookieValue">String obsahujici hodnotu cookie.</param>
    /// <param name="validator">Instance validatoru security cookie.</param>
    /// <returns>Vraci instanci uzivatele. Pokud byla cookie neplatna, vraci instanci neplatneho uzivatele (IsAuthenticated=false)</returns>
    MpssUser Decode(string cookieValue, CookieValidator validator);

    MpssUser Decode(string cookieValue, CookieValidator validator, out SecurityCookie cookie);
}
