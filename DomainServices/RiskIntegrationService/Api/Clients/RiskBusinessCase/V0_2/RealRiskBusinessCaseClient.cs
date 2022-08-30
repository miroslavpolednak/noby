using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;
using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2;

internal sealed class RealRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
    public async Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _createCaseUrl, request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationCreate>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCase), nameof(LoanApplicationCreate));

        return result;
    }

    public async Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _createCaseAssessmentUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Identified>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCaseAssessment), nameof(Identified));

        return result;
    }

    public async Task<RiskBusinessCaseCommand> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _createCaseAssessmentAsynchronousUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<RiskBusinessCaseCommand>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(CreateCaseAssessmentAsynchronous), nameof(RiskBusinessCaseCommand));

        return result;
    }

    public async Task<LoanApplicationCommit> CommitCase(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + string.Format(default, _commitCaseUrl, riskBusinessCaseId), request, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationCommit>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(CommitCase), nameof(LoanApplicationCommit));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _createCaseUrl = "/riskBusinessCase";
    const string _createCaseAssessmentUrl = "/riskBusinessCase/{0}/assessment";
    const string _createCaseAssessmentAsynchronousUrl = "/assessment/command?riskBusinessCaseId={0}";
    const string _commitCaseUrl = "/riskBusinessCase/{0}/commitment";

    public RealRiskBusinessCaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
