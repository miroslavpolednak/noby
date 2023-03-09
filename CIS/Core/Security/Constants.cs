namespace CIS.Core.Security;

/// <summary>
/// Konstanty pro nastavení auth providerů atd.
/// </summary>
public sealed class SecurityConstants
{
    /// <summary>
    /// Klíč pro HTTP hlavičku s informací o v33id aktuálně přihlášeného uživatele.
    /// </summary>
    public const string ContextUserHttpHeaderUserIdKey = "noby-user-id";

    /// <summary>
    /// Klíč pro HTTP hlavičku s informací o loginu (CAAS) aktuálně přihlášeného uživatele.
    /// </summary>
    public const string ContextUserHttpHeaderUserIdentKey = "noby-user-ident";

    /// <summary>
    /// Type claimu, který obsahuje login (CAAS login) přihlášeného uživatele
    /// </summary>
    public const string ClaimNameIdent = "noby-user-ident";

    public const string ClaimNameId = "noby-user-id";
}
