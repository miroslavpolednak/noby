namespace CIS.Core.Configuration;

/// <summary>
/// Nastavení CORS pro REST/Webapi projekty.
/// </summary>
public interface ICorsConfiguration
{
    /// <summary>
    /// true = zapne CORS middleware v Webapi projektu
    /// </summary>
    bool EnableCors { get; set; }

    /// <summary>
    /// Seznam povolených origins
    /// </summary>
    /// <example>https://localhost:8080, https://dev.noby.cz</example>
    string[]? AllowedOrigins { get; set; }
}