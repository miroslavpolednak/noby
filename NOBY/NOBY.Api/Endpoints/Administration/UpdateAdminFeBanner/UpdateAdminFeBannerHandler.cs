using Microsoft.EntityFrameworkCore;
using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Administration.UpdateAdminFeBanner;

internal sealed class UpdateAdminFeBannerHandler(
    Database.FeApiDbContext _dbContext,
    FeBannersService _feBannersService)
    : IRequestHandler<AdminUpdateAdminFeBannerRequest>
{
    public async Task Handle(AdminUpdateAdminFeBannerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.FeBanners
            .Where(t => t.FeBannerId == request.FeBannerId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CisNotFoundException(0, "Banner not found");

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.VisibleFrom = request.VisibleFrom;
        entity.VisibleTo = request.VisibleTo;
        entity.Severity = (int)request.Severity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _feBannersService.ClearCache();
    }
}
