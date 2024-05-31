using CIS.Core.Data;
using SharedTypes.GrpcTypes;
using CIS.Infrastructure.Data;

namespace DomainServices.ProductService.Api.Endpoints.SearchProducts;

internal sealed class SearchProductsHandler(IConnectionProvider _connectionProvider)
    : IRequestHandler<SearchProductsRequest, SearchProductsResponse>
{
    public async Task<SearchProductsResponse> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var result = await searchProducts(request.Identity, cancellationToken);

        return new SearchProductsResponse
        {
            Products = { result }
        };
	}

	private Task<List<SearchProductsResponse.Types.SearchProductsItem>> searchProducts(Identity? identity, CancellationToken cancellationToken)
	{
		string query = """
        select
            u.Id as CaseId,
            vu.VztahId as ContractRelationshipTypeId
        from [dbo].[VztahUver] vu
        inner join [dbo].[Partner] p on p.Id = vu.PartnerId
        inner join [dbo].[Uver] u on u.Id = vu.UverId
        where  vu.Neaktivni = 0 
               and u.Neaktivni = 0 
               and ((@schema=2 and p.KBId=@id) or (@schema=1 and p.Id=@id))

        union all

        select
            s.Id as CaseId,
            vs.VztahId as ContractRelationshipTypeId
        from [dbo].[VztahSporeni] vs
        inner join [dbo].[Partner] p on p.Id = vs.PartnerId
        inner join [dbo].[Sporeni] s on s.Id = vs.SporeniId 
        where  vs.Neaktivni = 0 and ((@schema=2 and p.KBId=@id) or (@schema=1 and p.Id=@id))
        """;

		if (identity is not null)
		{
			return _connectionProvider.ExecuteDapperRawSqlToListAsync<SearchProductsResponse.Types.SearchProductsItem>(query, new { schema = (int)identity.IdentityScheme, id = identity.IdentityId }, cancellationToken);
		}

		throw new NotImplementedException();
	}
}
