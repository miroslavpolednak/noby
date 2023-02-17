using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListHandler
    : BaseMortgageHandler, IRequestHandler<Contracts.GetProductListRequest, GetProductListResponse>
{
    #region Construction

    public GetProductListHandler(
      ICodebookServiceClients codebookService,
      Database.LoanRepository repository,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ILogger<GetProductListHandler> logger) : base(codebookService, repository, null, logger) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.


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