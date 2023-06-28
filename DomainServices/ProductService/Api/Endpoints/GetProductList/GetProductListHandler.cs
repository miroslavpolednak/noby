using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListHandler
    : IRequestHandler<Contracts.GetProductListRequest, GetProductListResponse>
{
    #region Construction
    private readonly Database.LoanRepository _repository;

    public GetProductListHandler(Database.LoanRepository repository)
    {
        _repository = repository;
    }

    #endregion

    public async Task<GetProductListResponse> Handle(Contracts.GetProductListRequest request, CancellationToken cancellation)
    {
        // create response
        var model = new GetProductListResponse();

        // add mortgage product if exists
        var loan = await _repository.GetLoan(request.CaseId, cancellation);
        if (loan != null)
        {
            model.Products.Add(new GetProductListItem
            {
                ProductId = request.CaseId,
                ProductTypeId = loan.KodProduktyUv.GetValueOrDefault()
            });
        }

        return model;
    }

}