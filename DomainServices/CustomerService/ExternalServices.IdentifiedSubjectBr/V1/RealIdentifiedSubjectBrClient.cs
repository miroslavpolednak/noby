using System.Net;
using CIS.Core.Exceptions;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class RealIdentifiedSubjectBrClient : IIdentifiedSubjectBrClient
{
    public async Task<CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _baseAddress + (hardCreate ? "?hardCreate=true" : ""), request, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<CreateIdentifiedSubjectResponse>(StartupExtensions.ServiceName, response, cancellationToken));
    }

    public async Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"{_httpClient.BaseAddress}{_baseAddress}/{customerId}", request, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new CisExtServiceValidationException($"{error?.Message}: {error?.Detail}"),
                HttpStatusCode.NotFound => new CisNotFoundException(11000, "Customer", customerId),
                _ => new CisExtServiceValidationException(
                    $"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}")
            };
        }
    }

    const string _baseAddress = "/v1/identified-subject";

    private readonly HttpClient _httpClient;
    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
        => _httpClient = httpClient;
}