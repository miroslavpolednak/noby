
namespace ExternalServices.Party.V1.Clients;
internal class MockPartyClient : IPartyClient
{
    public Task<GetRESInfoResponse1> GetRESInfo(string? countryCode, string cin, string v33UserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SuggestJuridicalPersonsResponse1> SuggestJuridicalPersons(string? countryCode, string searchText, string v33UserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
