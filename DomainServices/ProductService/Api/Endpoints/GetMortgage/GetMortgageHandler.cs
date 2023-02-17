using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageHandler
    : BaseMortgageHandler, IRequestHandler<GetMortgageRequest, GetMortgageResponse>
{
    #region Construction

    public GetMortgageHandler(
        ICodebookServiceClients codebookService,
        Database.LoanRepository repository,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ILogger<GetMortgageHandler> logger) : base(codebookService, repository, null, logger) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    #endregion

    public async Task<GetMortgageResponse> Handle(GetMortgageRequest request, CancellationToken cancellation)
    {
        var loan = await _repository.GetLoan(request.ProductId, cancellation);

        if (loan == null)
        {
            throw new CisNotFoundException(12001, nameof(Database.Entities.Loan), request.ProductId);
        }

        var relationships = await _repository.GetRelationships(request.ProductId, cancellation);

        var mortgage = loan.ToMortgage(relationships);

        return new GetMortgageResponse { Mortgage = mortgage };
    }

}