using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal sealed class RealRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
    public async Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
    {
        return null;
    }

    public async Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
    {
        _logger.ExtServiceRequest(RiskBusinessCaseStartupExtensions.ServiceName, _createCaseUrl, request);

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _createCaseUrl, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoanApplicationCreate>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, RiskBusinessCaseStartupExtensions.ServiceName, _createCaseUrl, nameof(LoanApplicationCreate));

            _logger.ExtServiceResponse(RiskBusinessCaseStartupExtensions.ServiceName, _createCaseUrl, response);
            return result;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            // asi validacni chyba?
            throw new CisValidationException(0, "validatce");
        }
        else
        {
            // 500?
            throw new ServiceCallResultErrorException(0, "chyba?");
        }
    }

    private readonly HttpClient _httpClient;
    private readonly ILogger<RealRiskBusinessCaseClient> _logger;

    const string _caseCommitmentsUrl = "/riskBusinessCase/{0}/commitments";
    const string _createCaseUrl = "/riskBusinessCase";

    public RealRiskBusinessCaseClient(HttpClient httpClient, ILogger<RealRiskBusinessCaseClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
