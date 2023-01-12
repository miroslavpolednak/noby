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
            .PostAsJsonAsync(_httpClient.BaseAddress + "/public/v1/identified-subject" + (hardCreate ? "?hardCreate=true" : ""), request, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<CreateIdentifiedSubjectResponse>(StartupExtensions.ServiceName, response, cancellationToken));
    }

    public async Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"{_httpClient.BaseAddress}/public/v1/identified-subject/{customerId}", request, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new CisExtServiceValidationException($"{error?.Message}: {error?.Detail}"),
                // todo: error code
                HttpStatusCode.NotFound => new CisNotFoundException(9999999, "customer", customerId),
                _ => new CisExtServiceValidationException(
                    $"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}")
            };
        }
    }

    private readonly HttpClient _httpClient;
    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
        => _httpClient = httpClient;
}