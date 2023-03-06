namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

internal interface IServiceWrapper
{
    DataSource DataSource { get; }

    Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
}