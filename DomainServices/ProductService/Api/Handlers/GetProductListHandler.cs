using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetProductListHandler
    : IRequestHandler<Dto.GetProductListMediatrRequest, GetProductListResponse>
{
    #region Construction

    private readonly ILogger<GetProductListHandler> _logger;

    public GetProductListHandler(
        ILogger<GetProductListHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetProductListResponse> Handle(Dto.GetProductListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetProductListHandler));

        // TODO:
        var model = new GetProductListResponse
        {
            
        };

        return model;
    }
  
}