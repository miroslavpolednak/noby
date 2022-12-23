using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class RealIdentifiedSubjectBrClient
    : IIdentifiedSubjectBrClient
{
    public async Task<Contracts.CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(Contracts.IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/public/v1/identified-subject" + (hardCreate ? "?hardCreate=true" : ""), request, cancellationToken)
            .ConfigureAwait(false);

        return (await Common.Helpers.ProcessResponse<CreateIdentifiedSubjectResponse>(StartupExtensions.ServiceName, response, cancellationToken));
    }

    private readonly HttpClient _httpClient;
    public RealIdentifiedSubjectBrClient(HttpClient httpClient)
        => _httpClient = httpClient;
}