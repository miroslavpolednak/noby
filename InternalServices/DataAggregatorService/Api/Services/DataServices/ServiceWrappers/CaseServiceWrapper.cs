﻿using DomainServices.CaseService.Clients.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CaseServiceWrapper : IServiceWrapper
{
    private readonly ICaseServiceClient _caseService;

    public CaseServiceWrapper(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }

    public DataService DataService => DataService.CaseService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCaseId();

        data.Case = await _caseService.GetCaseDetail(input.CaseId!.Value, cancellationToken);
    }
}