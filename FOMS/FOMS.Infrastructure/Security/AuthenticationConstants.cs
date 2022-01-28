namespace FOMS.Infrastructure.Security;

public class AuthenticationConstants
{
    /// <summary>
    /// Mock authentication scheme name. Also used in appsettings.json to specify Mock Authentication provider to be used.
    /// </summary>
    public const string MockAuthScheme = "FomsMockAuthentication";

    /// <summary>
    /// Simple login authentication scheme name.
    /// </summary>
    public const string SimpleLoginAuthScheme = "SimpleLoginAuthentication";
}
