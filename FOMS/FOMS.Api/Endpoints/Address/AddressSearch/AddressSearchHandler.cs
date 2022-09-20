namespace FOMS.Api.Endpoints.Address.AddressSearch;

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

        return result switch
        {
            SuccessfulServiceCallResult<List<ExternalServices.AddressWhisperer.Shared.FoundSuggestion>> t => new AddressSearchResponse 
            { 
                PageSize = request.PageSize, 
                Rows = t.Model.Select(x => new Dto.AddressLine
                {
                    Id = x.AddressId,
                    Title = x.Title
                }).ToList()
            },
            
            EmptyServiceCallResult => new AddressSearchResponse { PageSize = request.PageSize },
            
            _ => throw new NotImplementedException("AddressSearchHandler result not implemented")
        };
    }

    private ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient;
    private DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public AddressSearchHandler(ExternalServices.AddressWhisperer.V1.IAddressWhispererClient addressWhispererClient, DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _addressWhispererClient = addressWhispererClient;
        _codebookService = codebookService;
    }
}
