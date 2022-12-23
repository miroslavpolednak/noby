using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class RealIdentifiedSubjectBrClient
    : IIdentifiedSubjectBrClient
{
    public async Task<Contracts.CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(Contracts.IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/public/v1/identified-subject" + (hardCreate ? "?hardCreate=true" : ""), request, cancellationToken)
            .ConfigureAwait(false);

        if (response?.IsSuccessStatusCode ?? false)
        {
            return await response.Content.ReadFromJsonAsync<Contracts.CreateIdentifiedSubjectResponse>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CreateIdentifiedSubject), nameof(Contracts.CreateIdentifiedSubjectResponse));
        }
        else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadFromJsonAsync<Contracts.Error>(cancellationToken: cancellationToken);
            if (error != null)
                throw new CisExtServiceValidationException($"{error.Message}: {error.Detail}");
        }

        throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }

    private readonly HttpClient _httpClient;
    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
        => _httpClient = httpClient;
}