using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider")]
internal sealed class RealRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
    public async Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
    {
        _logger.ExtServiceRequest(RiskBusinessCaseStartupExtensions.ServiceName, _createCaseUrl, request);

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _createCaseUrl, request, cancellationToken);
        var result = await response.ToClientResponse<LoanApplicationCreate>(_createCaseUrl, _logger, cancellationToken);

        return result;
    }

    public async Task<Identified> CaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
    {
        string url = string.Format(_caseAssessmentUrl, riskBusinessCaseId);

        _logger.ExtServiceRequest(RiskBusinessCaseStartupExtensions.ServiceName, url, request);

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + url, request, cancellationToken);
        var result = await response.ToClientResponse<Identified>(url, _logger, cancellationToken);

        return result;
    }

    public async Task<RiskBusinessCaseCommand> CaseAssessmentAsync(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
    {
        string url = string.Format(_caseAssessmentAsync, riskBusinessCaseId);

        _logger.ExtServiceRequest(RiskBusinessCaseStartupExtensions.ServiceName, url, request);

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + url, request, cancellationToken);
        var result = await response.ToClientResponse<RiskBusinessCaseCommand>(url, _logger, cancellationToken);

        return result;
    }

    public async Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
    {
        string url = string.Format(_caseCommitmentsUrl, riskBusinessCaseId);

        _logger.ExtServiceRequest(RiskBusinessCaseStartupExtensions.ServiceName, url, request);

        var response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + url, request, cancellationToken);
        var result = await response.ToClientResponse<LoanApplicationCommit>(url, _logger, cancellationToken);

        return result;
    }

    private readonly HttpClient _httpClient;
    private readonly ILogger<RealRiskBusinessCaseClient> _logger;

    const string _createCaseUrl = "/riskBusinessCase";
    const string _caseAssessmentUrl = "/riskBusinessCase/{0}/assessment";
    const string _caseAssessmentAsync = "/assessment/command?riskBusinessCaseId={0}";
    const string _caseCommitmentsUrl = "/riskBusinessCase/{0}/commitment";

    public RealRiskBusinessCaseClient(HttpClient httpClient, ILogger<RealRiskBusinessCaseClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
