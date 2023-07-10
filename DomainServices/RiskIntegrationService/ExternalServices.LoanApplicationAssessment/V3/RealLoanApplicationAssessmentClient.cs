namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V3;

internal sealed class RealLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public async Task<Contracts.LoanApplicationAssessment> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
    {
        string path = requestedDetails != null && requestedDetails.Any()
            ? string.Format(default, _getUrlWithExpand, loanApplicationAssessmentId, string.Join("&expand=", requestedDetails))
            : string.Format(default, _getUrl, loanApplicationAssessmentId);

        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + path, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Contracts.LoanApplicationAssessment>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(ErrorCodeMapper.ServiceResponseDeserializationException, StartupExtensions.ServiceName, nameof(GetAssessment), nameof(Contracts.LoanApplicationAssessment));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _getUrl = "/loan-application-assessment/{0}";
    const string _getUrlWithExpand = "/loan-application-assessment/{0}?expand={1}";

    public RealLoanApplicationAssessmentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
