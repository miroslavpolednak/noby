using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class UpdateMortgageHandler
    : IRequestHandler<Dto.UpdateMortgageMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly ILogger<UpdateMortgageHandler> _logger;

    public UpdateMortgageHandler(
        ILogger<UpdateMortgageHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(UpdateMortgageHandler));

        // TODO:

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}