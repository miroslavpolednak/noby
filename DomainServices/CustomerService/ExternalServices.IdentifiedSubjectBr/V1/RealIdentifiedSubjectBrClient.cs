using System.Net;
using CIS.Core.Exceptions;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class RealIdentifiedSubjectBrClient : IIdentifiedSubjectBrClient
{
    private const string BaseAddress = "/v1/identified-subject";

    private readonly HttpClient _httpClient;
    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default)
    {
        var address = $"{_httpClient.BaseAddress}{BaseAddress}{(hardCreate ? "?hardCreate=true" : "")}";

        var response = await _httpClient.PostAsJsonAsync(address, request, cancellationToken);

        if (response.IsSuccessStatusCode)
            return new IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>
            {
                Result = await ParseResponse<CreateIdentifiedSubjectResponse>(response, cancellationToken)
            };

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return new IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>
            {
                Error = await ParseResponse<IdentifiedSubjectError>(response, cancellationToken)
            };

        throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }

    public async Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"{_httpClient.BaseAddress}{BaseAddress}/{customerId}", request, cancellationToken)
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

    private async Task<TResponse> ParseResponse<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken) =>
        await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken)
        ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(ParseResponse), nameof(TResponse));
}