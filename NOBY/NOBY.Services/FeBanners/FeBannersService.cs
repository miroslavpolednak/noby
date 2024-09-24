using LazyCache;
using Microsoft.EntityFrameworkCore;
using NOBY.ApiContracts;

namespace NOBY.Services.FeBanners;

[ScopedService, SelfService]
public sealed class FeBannersService(
    TimeProvider _timeProvider,
    Database.FeApiDbContext _dbContext,
    IAppCache _cache)
{
    public List<UsersGetCurrentBannerItem> GetBanners()
    {
        return _cache.GetOrAdd("FeBanners", () =>
        {
            var d = _timeProvider.GetLocalNow().DateTime;
            return _dbContext
                .FeBanners
                .AsNoTracking()
                .Where(t => t.VisibleFrom < d && t.VisibleTo > d)
                .OrderByDescending(t => t.Severity).ThenBy(t => t.VisibleTo)
                .Take(5)
                .Select(t => new UsersGetCurrentBannerItem
                {
                    Id = t.FeBannerId,
                    Description = t.Description,
                    Title = t.Title,
                    Severity = (FeBannerBaseItemSeverity)t.Severity
                })
                .ToList();
        }, DateTime.Now.AddMinutes(15));
    }

    public void ClearCache()
    {
        _cache.Remove("FeBanners");
    }
}
