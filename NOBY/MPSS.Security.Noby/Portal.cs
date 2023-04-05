using Microsoft.AspNetCore.Http;

namespace MPSS.Security.Noby;

public static class Portal
{
    /// <summary>
    /// Vytvoreni dummy uzivatele a zapise nove vytvorenou cookie do response
    /// </summary>
    /// <param name="cpm">CPM</param>
    /// <param name="icp">ICP</param>
    /// <param name="name">Jmeno uzivatele</param>
    /// <param name="id">v33id</param>
    /// <param name="s53id">s53id uzivatele - 0 pokud se nejedna o uzivatele z t01stat_mesic</param>
    /// <param name="v11id">v11id uzivatele - 0 pokud se nejedna o zamestnance</param>
    /// <param name="v03id">v03id uzivatele - 0 pokud se nejedna o agenturu</param>
    /// <param name="context">Aktualni HTTP kontext</param>
    public static string CreateCookieValue(
        string cpm, /* CPM uzivatele */
        string icp, /* ICP uzivatele */
        string name, /* jmeno a prijmeni uzivatele */
        int id, /* v33id uzivatele */
        int s53id, /* s53id uzivatele - 0 pokud se nejedna o uzivatele z t01stat_mesic */
        int v11id, /* v11id uzivatele - 0 pokud se nejedna o zamestnance */
        int v03id, /* v03id uzivatele - 0 pokud se nejedna o agenturu */
        int m17id,
        int brokerId,
        HttpContext context)
    {
        IdentityBase dummyIdentity = new IdentityBase(true);

        dummyIdentity.CPM = cpm;
        dummyIdentity.ICP = icp;
        dummyIdentity.Name = name;
        dummyIdentity.ID = id;
        dummyIdentity.LegacyS53ID = s53id;
        dummyIdentity.LegacyV03ID = v03id;
        dummyIdentity.LegacyV11ID = v11id;
        dummyIdentity.m17ID = m17id;
        dummyIdentity.BrokerId = brokerId;

        // vytvorit guid session
        dummyIdentity.SessionId = Guid.NewGuid().ToString();

        MpssUser user = new MpssUser(dummyIdentity);
        return CookieRepository.GetCookieForWrite(user, new CookieValidator(context), ConfigurationCache.Instance(null, null).Configuration);
    }

    public static void Init(int? environmentId, string securityConfigurationFile)
    {
        _ = ConfigurationCache.Instance(environmentId, securityConfigurationFile).Configuration;
    }

    public static Configuration Configuration
    {
        get
        {
            return ConfigurationCache.Instance(null, null).Configuration;
        }
    }
}
