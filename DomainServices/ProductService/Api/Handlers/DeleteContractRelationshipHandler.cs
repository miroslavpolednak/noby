using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class DeleteContractRelationshipHandler
    : IRequestHandler<Dto.DeleteContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly ILogger<DeleteContractRelationshipHandler> _logger;

    public DeleteContractRelationshipHandler(
        ILogger<DeleteContractRelationshipHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(DeleteContractRelationshipHandler));

        // TODO:

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}