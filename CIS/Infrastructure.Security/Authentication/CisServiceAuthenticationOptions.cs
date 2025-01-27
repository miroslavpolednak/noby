﻿using Microsoft.AspNetCore.Authentication;

namespace CIS.Infrastructure.Security;

public sealed class CisServiceAuthenticationOptions 
    : AuthenticationSchemeOptions
{
    /// <summary>
    /// Domena ve ktere je umisten autentizovany uzivatel. Napr. "vsskb\"
    /// Pozor, musi byt vcetne \ na konci
    /// </summary>
    public string? Domain { get; set; } = "vsskb";

    /// <summary>
    /// Adresa domenoveho serveru
    /// </summary>
    public string? AdHost { get; set; }

    /// <summary>
    /// Port na domenovem serveru, vychozi je 389
    /// </summary>
    public int AdPort { get; set; } = 389;

    /// <summary>
    /// True pokud se jedna o SSL connection
    /// </summary>
    public bool IsSsl { get; set; }
}
