using Microsoft.AspNetCore.Http;

namespace MPSS.Security.Noby;

/// <summary>
/// Repository pro praci s kolekci cookies.
/// </summary>
internal sealed class CookieRepository
{
    /// <summary>
    /// Vytvoreni instance HttpCookie pro zapsani do response
    /// </summary>
    /// <param name="user">Instance uzivatele (identity).</param>
    /// <param name="validator">Instance cookie validatoru.</param>
    /// <returns>Zasifrovana cookie pro ulozeni do response kolekce.</returns>
    public static string GetCookieForWrite(MpssUser user, CookieValidator validator, Configuration config)
    {
        // zasifrovani objektu
        ISecurityCookieFormatter formatter = SecurityCookieFactory.GetFormatter(config);
#pragma warning disable CS8604 // Possible null reference argument.
        return formatter.Encode(user.Identity as IdentityBase, validator);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    /// <para>Cte SecurityCookie soucasne prihlaseneho uzivatele.</para>
    /// <para>Zaroven overuje, zda pozadavek pochazi z povolene domeny - pokud ne, vraci prazdnou instanci uzivatele.</para>
    /// </summary>
    /// <param name="applicationId">ID volajici aplikace (0=neznama aplikace)</param>
    /// <returns>Vraci instanci uzivatele ze SecurityCookie. Pokud neni uzivatel autentifikovan, vraci IsAuthenticated=false.</returns>
    public static MpssUser GetSession(HttpContext context, string cookie, Configuration config)
    {
        // vytvorit cookie validator - trida obsahujici validacni informace o pozadavku
        CookieValidator validator = new CookieValidator(context);

        // dekodovani cookie
        ISecurityCookieFormatter formatter = SecurityCookieFactory.GetFormatter(Portal.Configuration);
        MpssUser user = formatter.Decode(cookie, validator);
        if (user == null)
            return null!;

        return user;
    }
}
