using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetMortgageHandler
    : IRequestHandler<Dto.GetMortgageMediatrRequest, GetMortgageResponse>
{
    #region Construction

    private readonly ILogger<GetMortgageHandler> _logger;

    public GetMortgageHandler(
        ILogger<GetMortgageHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetMortgageResponse> Handle(Dto.GetMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetMortgageHandler));

        // TODO:
        var model = new GetMortgageResponse
        {
            
        };

        return model;
    }
  
}