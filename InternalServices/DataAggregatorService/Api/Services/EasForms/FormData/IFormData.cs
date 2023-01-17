using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

internal interface IFormData
{
    AggregatedData AggregatedData { get; }

    Task LoadFormSpecificData(CancellationToken cancellationToken);
}