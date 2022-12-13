using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData.ProductRequest;

public interface IHouseholdData
{
    List<CustomerOnSA> CustomersOnSa { get; }
    List<Household> Households { get; }
    Dictionary<int, Income> Incomes { get; }
}