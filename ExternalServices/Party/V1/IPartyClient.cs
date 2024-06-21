using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Party.V1;
public interface IPartyClient : IExternalServiceClient
{
    const string Version = "V1";

    Task<GetRESInfoResponse1> GetRESInfo(string countryCode, string cin, string v33UserId, CancellationToken cancellationToken = default);

    Task<SuggestJuridicalPersonsResponse1> SuggestJuridicalPersons(string countryCode, string searchText, string v33UserId, CancellationToken cancellationToken = default);
}
