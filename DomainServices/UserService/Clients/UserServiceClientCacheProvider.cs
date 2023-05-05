using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.UserService.Clients;

internal sealed class UserServiceClientCacheProvider
{
    /// <summary>
    /// Instance cache pokud existuje
    /// </summary>
    public IDistributedCache? DistributedCacheInstance { get; set; }

    /// <summary>
    /// Pokud je k dispozici distribuovana cache a zaroven neni kesovani UserService vypnuto v appsettings.json
    /// </summary>
    public bool UseDistributedCache { get; set; }
}
