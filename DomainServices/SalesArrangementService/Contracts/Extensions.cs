using SharedTypes.Enums;

namespace DomainServices.SalesArrangementService.Contracts;

public static class Extensions
{
    public static bool IsProductSalesArrangement(this SalesArrangement sa)
    {
        return sa.SalesArrangementTypeId == (int)SalesArrangementTypes.Mortgage;
    }
}
