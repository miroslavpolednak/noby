using CIS.Foms.Types.Enums;

namespace DomainServices.RealEstateValuationService.Contracts;

public static class Helpers
{
    public static RealEstateTypes GetRealEstateType(RealEstateValuationListItem realEstateDetail)
        => realEstateDetail.RealEstateTypeId switch
        {
            1 or 2 or 3 or 5 or 6 => RealEstateTypes.Hf,
            9 => RealEstateTypes.Hff,
            4 or 7 => RealEstateTypes.P,
            _ => RealEstateTypes.O
        };

    public static int GetRealEstateTypeIcon(RealEstateValuationListItem realEstateDetail)
        => realEstateDetail.RealEstateTypeId switch
        {
            1 or 5 or 6 => 1,
            2 or 3 or 9 => 2,
            4 or 7 => 3,
            _ => 4
        };
}
