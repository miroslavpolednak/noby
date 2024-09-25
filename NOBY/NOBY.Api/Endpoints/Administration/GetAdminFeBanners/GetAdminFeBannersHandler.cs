using Microsoft.EntityFrameworkCore;

namespace NOBY.Api.Endpoints.Administration.GetAdminFeBanners;

internal sealed class GetAdminFeBannersHandler(NOBY.Database.FeApiDbContext _dbContext)
    : IRequestHandler<GetAdminFeBannersRequest, List<AdministrationFeBannerDetail>>
{
    public async Task<List<AdministrationFeBannerDetail>> Handle(GetAdminFeBannersRequest request, CancellationToken cancellationToken)
    {
        var response = await _dbContext
            .FeBanners
            .AsNoTracking()
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
            .ToListAsync(cancellationToken);

        return response;
    }
}
