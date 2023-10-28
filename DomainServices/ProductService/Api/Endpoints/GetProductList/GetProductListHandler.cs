namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListHandler : IRequestHandler<GetProductListRequest, GetProductListResponse>
{
    private readonly LoanRepository _repository;

    public GetProductListHandler(LoanRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<GetProductListResponse> Handle(GetProductListRequest request, CancellationToken cancellationToken)
    {
        // add mortgage product if exists
        var loan = await _repository.GetLoan(request.CaseId, cancellationToken);

        if (loan is null)
            return new GetProductListResponse();

        return new GetProductListResponse
        {
            Products =
            {
                new GetProductListItem
                {
                    ProductId = request.CaseId,
                    ProductTypeId = loan.ProductTypeId.GetValueOrDefault()
                }
            }
        };
    }

}