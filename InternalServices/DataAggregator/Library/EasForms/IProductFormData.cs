using CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest;

namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IProductFormData : IEasFormData
{
    IHouseholdData HouseholdData { get; }
}