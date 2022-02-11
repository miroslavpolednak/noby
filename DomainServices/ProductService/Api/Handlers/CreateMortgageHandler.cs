using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateMortgageHandler
    : IRequestHandler<Dto.CreateMortgageMediatrRequest, ProductIdReqRes>
{
    #region Construction

    private readonly ILogger<CreateMortgageHandler> _logger;

    public CreateMortgageHandler(
        ILogger<CreateMortgageHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<ProductIdReqRes> Handle(Dto.CreateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateMortgageHandler));

        // TODO:
        var model = new ProductIdReqRes
        {
            
        };

        return model;
    }
  
}