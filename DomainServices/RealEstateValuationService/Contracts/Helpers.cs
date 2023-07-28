using CIS.Foms.Types.Enums;

namespace DomainServices.RealEstateValuationService.Contracts;

public static class Helpers
{
    public static RealEstateTypes GetRealEstateType(IRealEstateValuationDetail realEstateDetail)
        => realEstateDetail.RealEstateTypeId switch
        {
            1 or 5 or 6 => RealEstateTypes.Hf,
            2 or 3 or 9 => RealEstateTypes.Hff,
            4 or 7 => RealEstateTypes.P,
            _ => RealEstateTypes.O
        };

    public static RealEstateTypeIcons GetRealEstateTypeIcon(int realEstateTypeId)
        => realEstateTypeId switch
        {
            1 or 5 or 6 => RealEstateTypeIcons.House,
            2 or 3 or 9 => RealEstateTypeIcons.LocationCity,
            4 or 7 => RealEstateTypeIcons.Custom,
            _ => RealEstateTypeIcons.Domain
        };
}

public enum RealEstateTypeIcons
{
    House = 1,
    LocationCity = 2,
    Custom = 3,
    Domain = 4
}