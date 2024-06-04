namespace NOBY.Api.Endpoints.Users.GetCurrentBannerList;

public sealed class GetCurrentBannerListResponse
{
    public NOBY.Dto.FeBannerItem[] Banners { get; set; } = null!;
}
