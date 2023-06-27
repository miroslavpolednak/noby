using CIS.Foms.Types.Enums;

namespace DomainServices.RealEstateValuationService.Contracts;

public static class Helpers
{
    public static RealEstateTypes GetRealEstateType(RealEstateValuationListItem realEstateDetail)
    {
        switch (realEstateDetail.RealEstateTypeId)
        {
            case 1 or 2 or 3 or 5 or 6:
                return RealEstateTypes.Hf;
            case 9:
                return RealEstateTypes.Hff;
            case 4 or 7:
                return RealEstateTypes.P;
            default:
                return RealEstateTypes.O;
        }
    }
}
