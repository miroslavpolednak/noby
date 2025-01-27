﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveHandler
    : IRequestHandler<_V2.LoanApplicationSaveRequest, _V2.LoanApplicationSaveResponse>
{
    public async Task<_V2.LoanApplicationSaveResponse> Handle(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // vytvorit c4m request
        var requestModel = await _requestMapper.MapToC4m(request, cancellation);

        // volani c4m
        var response = await _client.Save(requestModel, cancellation);

        bool responseVerPriority = requestModel.LoanApplicationHousehold?.All(t => t.CounterParty.All(x => x.Income?.EmploymentIncomes?.All(y => y.VerificationPriority == 1) ?? false)) ?? false;
        return new _V2.LoanApplicationSaveResponse
        {
            RiskSegment = responseVerPriority ? _V2.LoanApplicationRiskSegments.A : _V2.LoanApplicationRiskSegments.B
        };
    }

    private readonly Mappers.SaveRequestMapper _requestMapper;
    private readonly ExternalServices.LoanApplication.V3.ILoanApplicationClient _client;

    public SaveHandler(ExternalServices.LoanApplication.V3.ILoanApplicationClient client, Mappers.SaveRequestMapper requestMapper)
    {
        _requestMapper = requestMapper;
        _client = client;
    }
}
