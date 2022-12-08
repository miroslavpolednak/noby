﻿using DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1;

internal sealed class RealCustomersExposureClient
    : ICustomersExposureClient
{
    public async Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, C4mJsonOptions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationRelatedExposureResult>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(17001, StartupExtensions.ServiceName, nameof(Calculate), nameof(LoanApplicationRelatedExposureResult));

        return result;
    }

    private readonly HttpClient _httpClient;
    const string _calculateUrl = "/loan-application-exposure-calculation";

    public RealCustomersExposureClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
