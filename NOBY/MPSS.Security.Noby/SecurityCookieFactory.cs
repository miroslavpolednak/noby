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
    public static ISecurityCookieFormatter GetFormatter(Configuration c)
    {
        return new SecurityCookieStringFormatter(c);
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

    /// <summary>
    /// Overeni platnosti dane SecurityCookie.
    /// </summary>
    /// <param name="cookie">Deserializovany objekt SecurityCookie z kolekce cookies</param>
    /// <param name="validator">Instance validatoru security cookie.</param>
    /// <returns>Vraci true, pokud je dany objekt platny</returns>
    public static bool Check(SecurityCookie cookie, CookieValidator validator, Configuration config)
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        if (validator == null) // vypnout kontrolu validatorem
        {
            Log.LoggerFactory.WriteLog(null, "MPSS.Security.Core.SecurityCookieFactory", "Check", 5, "Validator is null", Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

            return true;
        }
        else
        {
            if (cookie == null)
            {
                Log.LoggerFactory.WriteLog(null, "MPSS.Security.Core.SecurityCookieFactory", "Check", 4, "Cookie is null", Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

                return false;
            }
            else
            {
                if (validator.DisableValidation)
                    return true;

                if (cookie.Identity == null 
                    //|| !validator.CheckIP(cookie.IP, config)
                    //! || cookie.UserAgent != validator.UserAgent 
                    || cookie.LastUpdate.AddMinutes(config.CookieExpiration) < DateTime.Now)
                {
                    string specialIPs = "";
                    if (config.CookieValidatorSpecialIPs != null)
                        specialIPs = string.Join(",", config.CookieValidatorSpecialIPs);

                    Log.LoggerFactory.WriteLog(null, "MPSS.Security.Core.SecurityCookieFactory", "Check", 3, string.Format("Cookie IP: {0} ({6}) [sIP={10}]; Validator IP: {2};LastUpdate: {1}; Expiration: {3} ({7}); Identity: {4}; UserAgent: {5} [cookie:{8} <-> validator:{9}]", cookie.IP, cookie.LastUpdate, validator.IP, config.CookieExpiration, cookie.Identity != null, cookie.UserAgent == validator.UserAgent, cookie.IP == validator.IP, cookie.LastUpdate.AddMinutes(config.CookieExpiration) >= DateTime.Now, cookie.UserAgent, validator.UserAgent, specialIPs), Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

                    return false;
                }
                else
                {
                    // maximalni doba prihlaseni uzivatele vyprsela
                    DateTime d = ((IdentityBase)cookie.Identity)._FirstLogin;
                    if (d.AddMinutes(config.MaxCookieAge) < DateTime.Now)
                    {
                        Log.LoggerFactory.WriteLog(null, "MPSS.Security.Core.SecurityCookieFactory", "Check", 3, string.Format("FirstLogin: {0}; MaxCookieAge: {1};", d, config.MaxCookieAge), Portal.Configuration.ApplicationId, Portal.Configuration.EnvironmentId);

                        return false;
                    }
                    else
                        return true;
                }
            }
        }
    }
}
