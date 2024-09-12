using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Administration.DeleteAdminFeBanner;

internal sealed class DeleteAdminFeBannerHandler(
    Database.FeApiDbContext _dbContext,
    FeBannersService _feBannersService)
    : IRequestHandler<DeleteAdminFeBannerRequest>
{
    public async Task Handle(DeleteAdminFeBannerRequest request, CancellationToken cancellationToken)
    {
        var entity = _dbContext.FeBanners.Find(request.FeBannerId);
        _dbContext.FeBanners.Remove(entity!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _feBannersService.ClearCache();
    }
}
