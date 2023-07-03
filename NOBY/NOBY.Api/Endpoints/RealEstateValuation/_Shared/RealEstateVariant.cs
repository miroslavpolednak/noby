namespace NOBY.Api.Endpoints.RealEstateValuation.Shared;

public enum RealEstateVariant
{
    HouseAndFlat,
    OnlyFlat,
    Parcel,
    Other
}

public static class RealEstateVariantHelper
{
    public static RealEstateVariant GetRealEstateVariant(int realEstateTypeId)
    {
        return realEstateTypeId switch
        {
            1 or 5 or 6 => RealEstateVariant.HouseAndFlat,
            2 or 3 or 9 => RealEstateVariant.OnlyFlat,
            4 or 7 => RealEstateVariant.Parcel,
            _ => RealEstateVariant.Other
        };
    }
}