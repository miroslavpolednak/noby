using Contracts = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Product.GetProductObligationList;

public class GetProductObligationListRequest : IRequest<List<Contracts.ProductObligation>>
{
    public long ProductId { get; }

    public GetProductObligationListRequest(long productId)
    {
        ProductId = productId;
    }
}