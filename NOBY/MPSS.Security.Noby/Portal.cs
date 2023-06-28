namespace MPSS.Security.Noby;

internal sealed class Portal
    : IPortal
{
    public string CreateCookieValue(
        string cpm, /* CPM uzivatele */
        string icp, /* ICP uzivatele */
        string name, /* jmeno a prijmeni uzivatele */
        int id, /* v33id uzivatele */
        int s53id, /* s53id uzivatele - 0 pokud se nejedna o uzivatele z t01stat_mesic */
        int v11id, /* v11id uzivatele - 0 pokud se nejedna o zamestnance */
        int v03id, /* v03id uzivatele - 0 pokud se nejedna o agenturu */
        int m17id,
        int brokerId,
        string kbuid)
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
        dummyIdentity.KbUid = kbuid;

        // vytvorit guid session
        dummyIdentity.SessionId = Guid.NewGuid().ToString();

        MpssUser user = new MpssUser(dummyIdentity);
        return CookieRepository.GetCookieForWrite(user, new CookieValidator(_contextAccessor.HttpContext!), _configuration, _logger);
    }

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly Configuration _configuration;
    private readonly ILogger<IPortal> _logger;

    public Portal(IHttpContextAccessor contextAccessor, Configuration configuration, ILogger<IPortal> logger)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
        _configuration = configuration;
    }
}
