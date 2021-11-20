using CIS.Infrastructure.Caching;

namespace DomainServices.CodebookService.Api;

/// <summary>
/// Konfigurace aplikace nacitana z appsettings.json z objektu { "AppConfiguration": ... }
/// </summary>
internal sealed class AppConfiguration
{
    public CacheTypes CacheType { get; set; }

    public string? CacheConnectionString { get; set; }
}
