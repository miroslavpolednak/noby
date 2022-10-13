using FOMS.Api.Endpoints.Product.GetProductObligationList.Dto;

namespace FOMS.Api.Endpoints.Product.GetProductObligationList;

public class GetProductObligationListRequest : IRequest<List<ProductObligation>>
{
    public long ProductId { get; }

    public GetProductObligationListRequest(long productId)
    {
        ProductId = productId;
    }
}