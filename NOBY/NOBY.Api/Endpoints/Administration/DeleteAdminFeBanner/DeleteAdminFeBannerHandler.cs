using Microsoft.EntityFrameworkCore;
using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Administration.DeleteAdminFeBanner;

internal sealed class DeleteAdminFeBannerHandler(
    Database.FeApiDbContext _dbContext,
    FeBannersService _feBannersService)
    : IRequestHandler<DeleteAdminFeBannerRequest>
{
    public async Task Handle(DeleteAdminFeBannerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .FeBanners
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.FeBannerId == request.FeBannerId, cancellationToken)
            ?? throw new CisNotFoundException(0, "Banner not found");

        _dbContext.FeBanners.Remove(entity!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _feBannersService.ClearCache();
    }
}
