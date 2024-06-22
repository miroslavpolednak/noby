namespace NOBY.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchHandler(
    ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient, 
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService)
        : IRequestHandler<AddressAddressSearchRequest, AddressAddressSearchResponse>
{
    public async Task<AddressAddressSearchResponse> Handle(AddressAddressSearchRequest request, CancellationToken cancellationToken)
    {
        // zeme z ciselniku
        string? country = null;
        if (request.CountryId.GetValueOrDefault() > 0)
            country = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.Id == request.CountryId)?.ShortName;
        try
        {
            var result = await _addressWhispererClient.GetSuggestions(request.SessionId, request.SearchText, request.PageSize, country, cancellationToken);

            return new AddressAddressSearchResponse
            {
                PageSize = request.PageSize,
                Rows = result.Select(x => new AddressAddressSearchAddressLine
                {
                    Id = x.AddressId,
                    Title = x.Title
                }).ToList()
            };
        }
        catch
        {
            throw new NobyValidationException(90020);
        }
    }
}
