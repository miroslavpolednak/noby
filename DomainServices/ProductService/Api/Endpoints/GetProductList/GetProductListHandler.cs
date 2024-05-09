namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetProductListRequest, GetProductListResponse>
{
    public async Task<GetProductListResponse> Handle(GetProductListRequest request, CancellationToken cancellationToken)
    {
        //TODO nechat vytvorit ekonomictejsi endpoint MPHOME
        var loan = await _mpHomeClient.GetMortgage(request.CaseId, cancellationToken);
        
        if (loan is null)
            return new GetProductListResponse();

        return new GetProductListResponse
        {
            Products =
            {
                new GetProductListItem
                {
                    ProductId = request.CaseId,
                    ProductTypeId = loan.ProductUvCode.GetValueOrDefault()
                }
            }
        };
    }

}