using CIS.InternalServices.DocumentDataAggregator.EasForms.FormData.ProductRequest;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public interface IProductFormData : IEasFormData
{
    SalesArrangement SalesArrangement { get; }

    IHouseholdData HouseholdData { get; }
}