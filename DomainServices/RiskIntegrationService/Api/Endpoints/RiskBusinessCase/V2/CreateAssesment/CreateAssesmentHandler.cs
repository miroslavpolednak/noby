﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using _cl = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesment;

internal sealed class CreateAssesmentHandler
    : IRequestHandler<_V2.RiskBusinessCaseCreateAssesmentRequest, _sh.LoanApplicationAssessmentResponse>
{
    public async Task<_sh.LoanApplicationAssessmentResponse> Handle(_V2.RiskBusinessCaseCreateAssesmentRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var requestModel = request.ToC4M(chanel);

        var response = await _client.CreateCaseAssessment(request.RiskBusinessCaseId, requestModel, cancellationToken);

        return response.ToRIP();
    }

    private readonly _cl.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CreateAssesmentHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        _cl.IRiskBusinessCaseClient client)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
    }
}
