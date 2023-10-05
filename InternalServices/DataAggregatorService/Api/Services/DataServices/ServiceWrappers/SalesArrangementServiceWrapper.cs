﻿using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class SalesArrangementServiceWrapper : IServiceWrapper
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public SalesArrangementServiceWrapper(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }

    public DataService DataService => DataService.SalesArrangementService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        data.SalesArrangement = await _salesArrangementService.GetSalesArrangement(input.SalesArrangementId!.Value, cancellationToken);

        input.CaseId = data.SalesArrangement.CaseId;
    }
}