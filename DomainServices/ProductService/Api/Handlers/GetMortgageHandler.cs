﻿using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Dto.GetMortgageMediatrRequest, GetMortgageResponse>
{
    #region Construction
   
    public GetMortgageHandler(
        ICodebookServiceClients codebookService,
        Repositories.LoanRepository repository,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ILogger<GetMortgageHandler> logger) : base(codebookService, repository, null, logger) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    #endregion

    public async Task<GetMortgageResponse> Handle(Dto.GetMortgageMediatrRequest request, CancellationToken cancellation)
    {
        var loan = await _repository.GetLoan(request.Request.ProductId, cancellation);

        if (loan == null)
        {
            throw new CisNotFoundException(12001, nameof(Repositories.Entities.Loan), request.Request.ProductId);
        }

        var relationships = await _repository.GetRelationships(request.Request.ProductId, cancellation);

        var mortgage = loan.ToMortgage(relationships);

        var map = await GetMapLoanTypeToProductTypeId();

        mortgage.ProductTypeId = map[loan.TypUveru];

        return new GetMortgageResponse{ Mortgage = mortgage};
    }
  
}