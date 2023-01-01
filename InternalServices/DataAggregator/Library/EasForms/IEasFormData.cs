using CIS.InternalServices.DataAggregator.Configuration.EasForm;
using CIS.InternalServices.DataAggregator.EasForms.FormData;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IEasFormData
{
    MockValues MockValues { get; }

    SalesArrangement SalesArrangement { get; }

    internal EasFormRequestType EasFormRequestType { get; }

    Task LoadAdditionalData();
}