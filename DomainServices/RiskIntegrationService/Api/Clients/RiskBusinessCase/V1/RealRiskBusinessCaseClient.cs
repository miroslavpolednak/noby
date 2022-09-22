using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal sealed class RealRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
    public async Task<Contracts.RiskBusinessCase> CreateCase(Create request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _createCaseUrl, request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Contracts.RiskBusinessCase>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCase), nameof(Contracts.RiskBusinessCase));

        return result;
    }

    public async Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, LoanApplicationAssessmentCreate request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _createCaseAssessmentUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Identified>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCaseAssessment), nameof(Identified));

        return result;
    }

    public async Task<RiskBusinessCaseCommitCommandInstance> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, RiskBusinessCaseCommitCommand request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _createCaseAssessmentAsynchronousUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<RiskBusinessCaseCommitCommandInstance>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCaseAssessmentAsynchronous), nameof(RiskBusinessCaseCommitCommandInstance));

        return result;
    }

    public async Task<RiskBusinessCaseCommit> CommitCase(string riskBusinessCaseId, RiskBusinessCaseCommitCreate request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _commitCaseUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<RiskBusinessCaseCommit>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, CreditWorthinessStartupExtensions.ServiceName, nameof(CommitCase), nameof(RiskBusinessCaseCommit));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _createCaseUrl = "/risk-business-case";
    const string _createCaseAssessmentUrl = "/risk-business-case/{0}/assessment";
    const string _createCaseAssessmentAsynchronousUrl = "/risk-business-case/{0}/command/create";
    const string _commitCaseUrl = "/risk-business-case/{0}/commitment";

    public RealRiskBusinessCaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
