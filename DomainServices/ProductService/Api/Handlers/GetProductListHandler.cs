﻿using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetProductListHandler
    : BaseMortgageHandler, IRequestHandler<Dto.GetProductListMediatrRequest, GetProductListResponse>
{
    #region Construction

    public GetProductListHandler(
      ICodebookServiceAbstraction codebookService,
      Repositories.LoanRepository repository,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ILogger<GetProductListHandler> logger) : base(codebookService, repository, null, logger) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.


    #endregion

    public async Task<GetProductListResponse> Handle(Dto.GetProductListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetProductListHandler));

        // create response
        var model = new GetProductListResponse();

        // add mortgage product if exists
        var loan = await _repository.GetLoan(request.Request.CaseId, cancellation);
        if (loan != null)
        {
            var map = await GetMapLoanTypeToProductTypeId();

            var product = new Product
            {
                ProductId = request.Request.CaseId,
                ProductTypeId = map[loan.TypUveru]
            };

            model.Products.Add(product);
        }

        return model;
    }
  
}