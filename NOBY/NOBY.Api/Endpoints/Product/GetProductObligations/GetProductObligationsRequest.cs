namespace NOBY.Api.Endpoints.Product.GetProductObligations;

internal sealed record GetProductObligationsRequest(long ProductId)
    : IRequest<List<ProductGetProductObligationListObligation>>
{
}