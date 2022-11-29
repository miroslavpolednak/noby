using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public interface IServiceFormData : IEasFormData
{
    SalesArrangement SalesArrangement { get; }
}