using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

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

        var responseVerPriority = requestModel.LoanApplicationHousehold?.SelectMany(t => t.CounterParty.SelectMany(x => x.Income?.EmploymentIncome?.Select(y => y.VerificationPriority))).ToList();
        return new _V2.LoanApplicationSaveResponse
        {
            //LoanApplicationId = response.Id,//TODO ResourceIdentifier
            LoanApplicationDataVersion = response.LoanApplicationDataVersion,
            RiskSegment = responseVerPriority is null ? _V2.LoanApplicationRiskSegments.B : (responseVerPriority.All(t => t.GetValueOrDefault()) ? _V2.LoanApplicationRiskSegments.A : _V2.LoanApplicationRiskSegments.B)
        };
    }

    private readonly Mappers.SaveRequestMapper _requestMapper;
    private readonly Clients.LoanApplication.V1.ILoanApplicationClient _client;

    public SaveHandler(Clients.LoanApplication.V1.ILoanApplicationClient client, Mappers.SaveRequestMapper requestMapper)
    {
        _requestMapper = requestMapper;
        _client = client;
    }
}
