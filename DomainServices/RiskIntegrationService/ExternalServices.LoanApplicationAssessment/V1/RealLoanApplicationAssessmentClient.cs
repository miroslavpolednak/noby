﻿using DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1;

internal sealed class RealLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public async Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
    {
        string path = requestedDetails != null && requestedDetails.Any()
            ? string.Format(default, _getUrlWithExpand, loanApplicationAssessmentId, string.Join("&expand=", requestedDetails))
            : string.Format(default, _getUrl, loanApplicationAssessmentId);

        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + path, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Identified>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExternalServiceResponseDeserializationException(ErrorCodeMapper.ServiceResponseDeserializationException, StartupExtensions.ServiceName, nameof(GetAssessment), nameof(Identified));

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
