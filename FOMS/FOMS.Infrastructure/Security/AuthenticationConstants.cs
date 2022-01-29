﻿namespace FOMS.Infrastructure.Security;

public static class AuthenticationConstants
{
    public const string CookieName = "nobyauth";

    /// <summary>
    /// Mock authentication scheme name. Also used in appsettings.json to specify Mock Authentication provider to be used.
    /// </summary>
    public const string MockAuthScheme = "FomsMockAuthentication";

    /// <summary>
    /// Simple login authentication scheme name.
    /// </summary>
    public const string SimpleLoginAuthScheme = "SimpleLoginAuthentication";

    public const string CaasAuthScheme = "CaasAuthentication";
}
