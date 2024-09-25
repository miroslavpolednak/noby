namespace NOBY.Api.Endpoints.Administration.DeleteAdminFeBanner;

internal sealed record DeleteAdminFeBannerRequest(int FeBannerId)
    : IRequest
{
}
