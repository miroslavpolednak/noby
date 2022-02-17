using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetMortgageHandler
    : IRequestHandler<Dto.GetMortgageMediatrRequest, GetMortgageResponse>
{
    #region Construction

    private readonly ILogger<GetMortgageHandler> _logger;
    protected readonly Repositories.LoanRepository _repository;

    public GetMortgageHandler(
        ILogger<GetMortgageHandler> logger,
         Repositories.LoanRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    #endregion

    public async Task<GetMortgageResponse> Handle(Dto.GetMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetMortgageHandler));

        var loan = await _repository.GetLoan(request.Request.ProductId, cancellation);
        var relationships = await _repository.GetRelationships(request.Request.ProductId, cancellation);

        var model = new GetMortgageResponse
        { 
            Mortgage = loan.ToMortgage(relationships)
        };

        return model;
    }
  
}