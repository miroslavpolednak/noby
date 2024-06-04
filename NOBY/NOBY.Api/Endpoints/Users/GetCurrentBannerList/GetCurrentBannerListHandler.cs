using NOBY.Services.FeBanners;

namespace NOBY.Api.Endpoints.Users.GetCurrentBannerList;

internal sealed class GetCurrentBannerListHandler(FeBannersService _feBanners)
    : IRequestHandler<GetCurrentBannerListRequest, GetCurrentBannerListResponse>
{
    public Task<GetCurrentBannerListResponse> Handle(GetCurrentBannerListRequest request, CancellationToken cancellationToken)
    {
        var list = new GetCurrentBannerListResponse
        {
            Banners = _feBanners.GetBanners()
        };

        return Task.FromResult(list); 
    }
}
