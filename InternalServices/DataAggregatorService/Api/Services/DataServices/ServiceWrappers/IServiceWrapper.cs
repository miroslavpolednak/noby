namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

internal interface IServiceWrapper
{
    Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
}