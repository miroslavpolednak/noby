using ExternalServices.RuianAddress.V1;
using System.Globalization;

namespace NOBY.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchHandler
    : IRequestHandler<AddressSearchRequest, AddressSearchResponse>
{
    public async Task<AddressSearchResponse> Handle(AddressSearchRequest request, CancellationToken cancellationToken)
    {
        // zeme z ciselniku
        var country = (await _codebookService.Countries(cancellationToken))
            .FirstOrDefault(t => t.Id == request.CountryId);

        var response = new AddressSearchResponse
        {
            PageSize = request.PageSize
        };

        try
        {
            if (country?.Id != 16)
            {
                var result = await _addressWhispererClient.GetSuggestions(request.SessionId, request.SearchText, request.PageSize, country?.ShortName, cancellationToken);

                response.Rows = result.Select(x => new Dto.AddressLine
                {
                    Id = x.AddressId,
                    Title = x.Title
                }).ToList();
            }
            else
            {
                var result = await _ruianAddress.FindAddresses(request.SearchText, request.PageSize, cancellationToken);

                response.Rows = result.Select(t => new Dto.AddressLine
                {
                    Id = t.Id.ToString(CultureInfo.InvariantCulture),
                    Title = t.FullAddress ?? ""
                }).ToList();
            }
        }
        catch
        {
            throw new NobyValidationException(90020);
        }

        return response;
    }

    private readonly IRuianAddressClient _ruianAddress;
    private readonly ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;

    public AddressSearchHandler(ExternalServices.AddressWhisperer.V1.IAddressWhispererClient addressWhispererClient, DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService, IRuianAddressClient ruianAddress)
    {
        _addressWhispererClient = addressWhispererClient;
        _codebookService = codebookService;
        _ruianAddress = ruianAddress;
    }
}
