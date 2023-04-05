using Microsoft.AspNetCore.Http;

namespace MPSS.Security.Noby;

/// <summary>
/// Kontajner pro informace potrebne pro validaci cookie.
/// </summary>
internal class CookieValidator
{
    /// <summary>
    /// IP adresa klienta.
    /// </summary>
    public string IP;

    /// <summary>
    /// Informace o prohlizeci klienta.
    /// </summary>
    public string UserAgent;

    /// <summary>
    /// Nepouzivat, pouze pro testovaci ucely!
    /// </summary>
    public bool DisableValidation;

    public CookieValidator(HttpContext context)
    {
        this.DisableValidation = false;
        this.IP = context.Connection.RemoteIpAddress.ToString();
        this.UserAgent = context.Request.Headers["User-Agent"].ToString();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CookieValidator()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.DisableValidation = false;
    }

    /// <summary>
    /// Kontrola aktualni IP adresy vuci IP adrese posledniho pozadavku
    /// </summary>
    /// <param name="cookieIP">IP adresa posledniho pozadavku</param>
    public bool CheckIP(string cookieIP, Configuration config)
    {
        if (DisableValidation)
            return true;

        if (cookieIP.Equals(this.IP))
            return true;
        else if (config.CookieValidatorSpecialIPs == null)
            return false;
        else if (config.CookieValidatorSpecialIPs.Contains(this.IP)) // aktualni IP je duveryhodna
            return true;
        else if (config.CookieValidatorSpecialIPs.Contains(cookieIP)) // predchozi IP je duveryhodna
            return true;
        else
            return false;
    }
}
