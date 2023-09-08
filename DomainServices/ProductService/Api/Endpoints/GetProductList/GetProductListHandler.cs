using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListHandler
    : IRequestHandler<GetProductListRequest, GetProductListResponse>
{
    private readonly Database.LoanRepository _repository;

    public GetProductListHandler(Database.LoanRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<GetProductListResponse> Handle(GetProductListRequest request, CancellationToken cancellation)
    {
        var response = new GetProductListResponse();

        // add mortgage product if exists
        var loan = await _repository.GetLoan(request.CaseId, cancellation);
        if (loan != null)
        {
            response.Products.Add(new GetProductListItem
            {
                ProductId = request.CaseId,
                ProductTypeId = loan.ProductTypeId.GetValueOrDefault()
            });
        }

        return response;
    }

}