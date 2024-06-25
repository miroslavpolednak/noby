
using System.Reflection.Metadata.Ecma335;

namespace ExternalServices.Party.V1.Clients;
internal class MockPartyClient : IPartyClient
{
    public Task<GetRESInfoResponse1> GetRESInfo(string? countryCode, string cin, string v33UserId, CancellationToken cancellationToken = default)
    {

        return Task.FromResult(new GetRESInfoResponse1 { getRESInfoResponse = new GetRESInfoResponse { juridicalPerson = null } });
    }

    public Task<SuggestJuridicalPersonsResponse1> SuggestJuridicalPersons(string? countryCode, string searchText, string v33UserId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new SuggestJuridicalPersonsResponse1 { suggestJuridicalPersonsResponse = new SuggestJuridicalPersonsResponse { juridicalPersonList = [] } });
    }
}
