using LazyCache;
using Microsoft.EntityFrameworkCore;
using NOBY.Dto;

namespace NOBY.Services.FeBanners;

[ScopedService, SelfService]
public sealed class FeBannersService(
    TimeProvider _timeProvider,
    Database.FeApiDbContext _dbContext,
    IAppCache _cache)
{
    public FeBannerItem[] GetBanners()
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
                .Select(t => new FeBannerItem
                {
                    Text = t.Text,
                    Title = t.Title,
                    Severity = (FeBannerItem.FeBannersSeverity)t.Severity
                })
                .ToArray();
        }, DateTime.Now.AddMinutes(5));
    }
}
