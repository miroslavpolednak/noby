﻿using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;
using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1;

internal sealed class RealCustomersExposureClient
    : ICustomersExposureClient
{
    public async Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationRelatedExposureResult>(_jsonOptions, cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), nameof(LoanApplicationRelatedExposureResult));

        return result;
    }

    private readonly HttpClient _httpClient;
    const string _calculateUrl = "/loan-application-exposure-calculation";
    private static System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString //TODO odstranit az c4m opravi format cisel
    };

    public RealCustomersExposureClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    static RealCustomersExposureClient()
    {
        _jsonOptions.Converters.Add(new CIS.Infrastructure.Json.DateTimeOffsetConverterUsingDateTimeParse()); //TODO odstranit az c4m opravi format cisel
    }
}
