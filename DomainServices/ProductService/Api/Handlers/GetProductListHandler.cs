using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetProductListHandler
    : IRequestHandler<Dto.GetProductListMediatrRequest, GetProductListResponse>
{
    #region Construction

    private readonly ILogger<GetProductListHandler> _logger;
    protected readonly Repositories.LoanRepository _repository;

    public GetProductListHandler(
        ILogger<GetProductListHandler> logger,
        Repositories.LoanRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    #endregion

    public async Task<GetProductListResponse> Handle(Dto.GetProductListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetProductListHandler));

        // create response
        var model = new GetProductListResponse();

        // add mortgage product if exists
        var loanExists = await _repository.ExistsLoan(request.Request.CaseId, cancellation);

        if (loanExists)
        {
            var product = new Product
            {
                ProductId = request.Request.CaseId,
                ProductTypeId = 99999, //TODO: find out ProductTypeId by ProductTypeCategory Mortgage
            };

            model.Products.Add(product);
        }

        return model;
    }
  
}