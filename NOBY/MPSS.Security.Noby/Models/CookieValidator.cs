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
}
