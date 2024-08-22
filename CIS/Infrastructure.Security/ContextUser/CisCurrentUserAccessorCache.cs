using CIS.Core.Security;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisCurrentUserAccessorCache(DomainServices.UserService.Clients.v1.IUserServiceClient _userService)
{
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private static readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new();

    public async Task<CisUserDetails> GetUser(int userId, CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue(userId, out CisUserDetails? cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(userId, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync(cancellationToken);
            try
            {
                if (!_cache.TryGetValue(userId, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    var userInstance = await _userService.GetUserBasicInfo(userId, cancellationToken);
                    cacheEntry = new CisUserDetails
                    {
                        DisplayName = userInstance.DisplayName
                    };

                    _cache.Set(userId, cacheEntry, new MemoryCacheEntryOptions
                    {
                        Size = 300,
                        SlidingExpiration = TimeSpan.FromMinutes(5),
                        Priority = CacheItemPriority.High
                    });
                }
            }
            finally
            {
                mylock.Release();
            }
        }

        return cacheEntry!;
    }
}
