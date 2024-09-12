namespace NOBY.Api.Endpoints.Administration.GetAdminFeBanner;

internal sealed record GetAdminFeBannerRequest(int FeBannerId)
    : IRequest<AdministrationFeBannerDetail>
{
}
