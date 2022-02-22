using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Dto.GetMortgageMediatrRequest, GetMortgageResponse>
{
    #region Construction
   
    public GetMortgageHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ILogger<GetMortgageHandler> logger) : base(codebookService, repository, null, logger) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    #endregion

    public async Task<GetMortgageResponse> Handle(Dto.GetMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetMortgageHandler));

        var loan = await _repository.GetLoan(request.Request.ProductId, cancellation);

        if (loan == null)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Loan), request.Request.ProductId); //TODO: error code
        }

        var relationships = await _repository.GetRelationships(request.Request.ProductId, cancellation);

        var mortgage = loan.ToMortgage(relationships);

        var map = await GetMapLoanTypeToProductTypeId();

        mortgage.ProductTypeId = map[loan.TypUveru];

        return new GetMortgageResponse{ Mortgage = mortgage};
    }
  
}