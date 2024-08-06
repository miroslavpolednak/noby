using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.SearchProducts;

internal sealed class SearchProductsHandler(IMpHomeClient _mpHomeClient, ICodebookServiceClient _codebookService)
    : IRequestHandler<SearchProductsRequest, SearchProductsResponse>
{
    public async Task<SearchProductsResponse> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var mpHomeRequest = new CaseSearchRequest
        {
            KbId = request.Identity.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb ? request.Identity.IdentityId : null,
            PartnerId = request.Identity.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp ? request.Identity.IdentityId : null
        };
        var results = await _mpHomeClient.SearchCases(mpHomeRequest, cancellationToken);

        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        return new SearchProductsResponse
        {
            Products = 
            {
                results?.Where(t => productTypes.Any(p => p.Id == t.ProductTypeId)).Select(t => new SearchProductsResponse.Types.SearchProductsItem
                {
                    CaseId = t.CaseId,
                    ContractRelationshipTypeId = t.ContractRelationshipTypeId.GetValueOrDefault()
                })
                .ToArray()
            }
        };
	}
}
