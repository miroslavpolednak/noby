using CIS.Core.Security;
using DomainServices.CodebookService.Clients;
using ExternalServices.Party.V1;

namespace NOBY.Api.Endpoints.Party.SearchParties;

internal sealed class SearchPartiesHandler(
    ICodebookServiceClient _codebookService,
    IPartyClient _party,
    ICurrentUserAccessor _currentUser)
    : IRequestHandler<PartySearchPartiesRequest, List<PartySearchPartiesResponse>>
{
    public async Task<List<PartySearchPartiesResponse>> Handle(PartySearchPartiesRequest request, CancellationToken cancellationToken)
    {
        List<PartySearchPartiesResponse> resultList = [];

        var countryCode = (await _codebookService.Countries(cancellationToken)).SingleOrDefault(r => r.Id == request.CountryId)?.ShortName;

        if (countryCode != "CZ" && countryCode != "SK")
        {
            return resultList;
        }

        if (!string.IsNullOrWhiteSpace(request.Cin))
        {
            var reSInfoResponse = await _party.GetRESInfo(countryCode!, request.Cin!, _currentUser.User?.Login!, cancellationToken);
            if (reSInfoResponse.getRESInfoResponse.juridicalPerson is not null)
            {
                resultList.Add(new()
                {
                    Cin = request.Cin,
                    Name = reSInfoResponse.getRESInfoResponse.juridicalPerson.name
                });
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var suggestJuridicalPersonsResponse = await _party.SuggestJuridicalPersons(countryCode ?? string.Empty, request.SearchText, _currentUser.User?.Login!, cancellationToken);

            if (suggestJuridicalPersonsResponse.suggestJuridicalPersonsResponse.pagingResult.numberOfEntriesTotal > 0)
            {
                resultList.AddRange(suggestJuridicalPersonsResponse.suggestJuridicalPersonsResponse.juridicalPersonList
               .Select(s => new PartySearchPartiesResponse
               {
                   Name = s.name,
                   Cin = s.orgIdentification
               }));
            }
        }

        //Merge results to contain each juridical person only once
        return resultList.GroupBy(g => g.Cin).Select(s => new PartySearchPartiesResponse
        {
            Cin = s.Key,
            Name = s.First().Name
        }).ToList();
    }
}
