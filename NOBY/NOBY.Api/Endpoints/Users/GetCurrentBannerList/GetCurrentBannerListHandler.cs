using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Users.GetCurrentBannerList;

internal sealed class GetCurrentBannerListHandler(FeBannersService _feBanners)
    : IRequestHandler<GetCurrentBannerListRequest, UsersGetCurrentBannerListResponse>
{
    public Task<UsersGetCurrentBannerListResponse> Handle(GetCurrentBannerListRequest request, CancellationToken cancellationToken)
    {
        var list = new UsersGetCurrentBannerListResponse
        {
            Banners = _feBanners.GetBanners()
        };

        return Task.FromResult(list); 
    }
}
