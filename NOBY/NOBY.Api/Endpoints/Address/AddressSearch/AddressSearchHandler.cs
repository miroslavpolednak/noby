namespace NOBY.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchHandler
    : IRequestHandler<AddressSearchRequest, AddressSearchResponse>
{
    public async Task<AddressSearchResponse> Handle(AddressSearchRequest request, CancellationToken cancellationToken)
    {
        // zeme z ciselniku
        string? country = null;
        if (request.CountryId.GetValueOrDefault() > 0)
            country = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.Id == request.CountryId)?.ShortName;

        var result = await _addressWhispererClient.GetSuggestions(request.SessionId, request.SearchText, request.PageSize, country, cancellationToken);

        return new AddressSearchResponse 
        { 
            PageSize = request.PageSize, 
            Rows = result.Select(x => new Dto.AddressLine
            {
                Id = x.AddressId,
                Title = x.Title
            }).ToList()
        };
    }

    private ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient;
    private DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;

    public AddressSearchHandler(ExternalServices.AddressWhisperer.V1.IAddressWhispererClient addressWhispererClient, DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        _addressWhispererClient = addressWhispererClient;
        _codebookService = codebookService;
    }
}
