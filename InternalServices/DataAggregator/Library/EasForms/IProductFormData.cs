using CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IProductFormData : IEasFormData
{
    SalesArrangement SalesArrangement { get; }

    IHouseholdData HouseholdData { get; }
}