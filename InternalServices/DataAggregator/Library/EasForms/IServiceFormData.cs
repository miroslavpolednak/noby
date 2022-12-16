using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IServiceFormData : IEasFormData
{
    SalesArrangement SalesArrangement { get; }
}