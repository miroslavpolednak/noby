using Microsoft.EntityFrameworkCore;

namespace NOBY.Api.Endpoints.Administration.GetAdminFeBanner;

internal sealed class GetAdminFeBannerHandler(Database.FeApiDbContext _dbContext)
    : IRequestHandler<GetAdminFeBannerRequest, AdministrationFeBannerDetail>
{
    public async Task<AdministrationFeBannerDetail> Handle(GetAdminFeBannerRequest request, CancellationToken cancellationToken)
    {
        return await _dbContext
           .FeBanners
           .AsNoTracking()
           .Where(x => x.FeBannerId == request.FeBannerId)
           .Select(x => new AdministrationFeBannerDetail
           {
               VisibleTo = x.VisibleTo,
               VisibleFrom = x.VisibleFrom,
               Title = x.Title,
               FeBannerId = x.FeBannerId,
               Description = x.Description,
               CreatedUserName = x.CreatedUserName ?? "Unknown",
               CreatedTime = x.CreatedTime,
               Severity = (FeBannerBaseItemSeverity)x.Severity
           })
           .FirstOrDefaultAsync(cancellationToken)
           ?? throw new CisNotFoundException(0, "Banner not found");
    }
}
