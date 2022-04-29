﻿namespace CIS.Core.Security;

/// <summary>
/// Helper pro ziskani fyzickeho uzivatele, ktery aplikaci/sluzbu vola
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Pokud je false, uzivatel neni autentikovan - User = null
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Instance uzivatele
    /// </summary>
    ICurrentUser? User { get; }
}