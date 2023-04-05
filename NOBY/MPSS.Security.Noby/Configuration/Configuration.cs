namespace MPSS.Security.Noby;

public class Configuration
{
    public int Server { get; internal set; }

    /// <summary>
    /// Druh aplikacniho prostredi.
    /// </summary>
    public int EnvironmentId { get; internal set; }

    /// <summary>
    /// v07id aplikace
    /// </summary>
    public int ApplicationId { get; internal set; }

    /// <summary>
    /// ConnectionString pro databazi s uzivateli
    /// </summary>
    internal string DbConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Connection string pro logovaci db.
    /// </summary>
    internal string LogConnectionString { get; set; } = string.Empty;

    #region portal URL
    /// <summary>
    /// URL vychozi stranky pro logovani uzivatele (v portalu).
    /// </summary>
    public string PMPLoginUrl { get; internal set; }

    /// <summary>
    /// URL hlavni obrazovky portalu.
    /// </summary>
    public string PMPMainUrl { get; internal set; }

    /// <summary>
    /// URL logoutovaciho skriptu.
    /// </summary>
    public string PMPLogoutUrl { get; internal set; }

    /// <summary>
    /// URL proxy stranky pro spusteni aplikace z SP
    /// </summary>
    public string PMPAppProxyUrl { get; internal set; }

    /// <summary>
    /// URL proxy stranky pro spusteni externi aplikace z SP
    /// </summary>
    public string PMPExtAppProxyUrl { get; internal set; }
    #endregion

    #region security cookie
    /// <summary>
    /// True pokud ma byt cookie ulozena na disk, jinak se jedna o session cookie.
    /// </summary>
    public bool PersistentCookie { get; internal set; }

    /// <summary>
    /// Maximalni doba prihlaseni uzivatele v minutach.
    /// </summary>
    public int MaxCookieAge { get; internal set; }

    /// <summary>
    /// Pokud je true SecurityCookie s sifrovanymi informacemi o uzivateli se bude odesilat pouze pokud se jedna o https protokol.
    /// </summary>
    public bool CookieOnlyHttps { get; internal set; }

    /// <summary>
    /// Path SecurityCookie s sifrovanymi informacemi o uzivateli.
    /// </summary>
    public string CookiePath { get; internal set; }

    /// <summary>
    /// Nazev SecurityCookie s sifrovanymi informacemi o uzivateli.
    /// </summary>
    public string CookieName { get; internal set; }

    /// <summary>
    /// Domena SecurityCookie s sifrovanymi informacemi o uzivateli.
    /// </summary>
    public string CookieDomainName { get; internal set; }

    /// <summary>
    /// Expirace SecurityCookie v minutach s sifrovanymi informacemi o uzivateli.
    /// </summary>
    public int CookieExpiration { get; internal set; }
    #endregion

    #region kryptovani
    internal string RijndealPassword { get; set; }
    internal byte[] RijndaelVector { get; set; }
    internal int RijndaelStrength { get; set; }
    internal int RijndaelPwdIterations { get; set; }
    internal byte[] RijndaelSalt { get; set; }
    #endregion

    /// <summary>
    /// Seznam IP adres pro ktere CookieValidator nekontroluje shodu s IP predchoziho pozadavku
    /// </summary>
    public string[] CookieValidatorSpecialIPs { get; internal set; } = null!;

    public int DbLogMinSeverity { get; set; }
}
