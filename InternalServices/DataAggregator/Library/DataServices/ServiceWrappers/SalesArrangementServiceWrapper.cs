using CIS.Core.Results;
using CIS.InternalServices.DataAggregator.Configuration;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class SalesArrangementServiceWrapper : IServiceWrapper
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public SalesArrangementServiceWrapper(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.SalesArrangementId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));

        var result = await _salesArrangementService.GetSalesArrangement(input.SalesArrangementId.Value, cancellationToken);
        
        data.SalesArrangement = ServiceCallResult.ResolveAndThrowIfError<SalesArrangement>(result);
    }
}