using NOBY.Dto.RealEstateValuation;

namespace NOBY.Api.Endpoints.RealEstateValuation;

internal static class RealEstateVariantHelper
{
    public static RealEstateVariants GetRealEstateVariant(int realEstateTypeId)
    {
        return realEstateTypeId switch
        {
            1 or 5 or 6 => RealEstateVariants.HouseAndFlat,
            2 or 3 or 9 => RealEstateVariants.OnlyFlat,
            4 or 7 => RealEstateVariants.Parcel,
            _ => RealEstateVariants.Other
        };
    }
}
