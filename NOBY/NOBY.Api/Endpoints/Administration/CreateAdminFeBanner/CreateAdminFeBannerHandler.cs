using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Administration.CreateAdminFeBanner;

internal sealed class CreateAdminFeBannerHandler(
    Database.FeApiDbContext _dbContext,
    FeBannersService _feBannersService)
    : IRequestHandler<AdminCreateAdminFeBannerRequest, int>
{
    public async Task<int> Handle(AdminCreateAdminFeBannerRequest request, CancellationToken cancellationToken)
    {
        var entity = new Database.Entities.FeBanner
        {
            VisibleFrom = request.VisibleFrom,
            VisibleTo = request.VisibleTo,
            Title = request.Title,
            Description = request.Description,
            Severity = (int)request.Severity
        };

        _dbContext.FeBanners.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _feBannersService.ClearCache();

        return entity.FeBannerId;
    }
}
