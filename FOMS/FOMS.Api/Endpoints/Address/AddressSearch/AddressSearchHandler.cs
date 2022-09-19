namespace FOMS.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchHandler
    : IRequestHandler<AddressSearchRequest, AddressSearchResponse>
{
    public async Task<AddressSearchResponse> Handle(AddressSearchRequest request, CancellationToken cancellationToken)
    {
        var result = await _addressWhispererClient.GetSuggestions(request.SessionId, request.SearchText, request.PageSize, request.CountryId);

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
            
            EmptyResult => new AddressSearchResponse { PageSize = request.PageSize },
            
            _ => throw new NotImplementedException("AddressSearchHandler result not implemented")
        };
    }

    private ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient;

    public AddressSearchHandler(ExternalServices.AddressWhisperer.V1.IAddressWhispererClient addressWhispererClient)
    {
        _addressWhispererClient = addressWhispererClient;
    }
}
