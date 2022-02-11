using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateContractRelationshipHandler
    : IRequestHandler<Dto.CreateContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly ILogger<CreateContractRelationshipHandler> _logger;

    public CreateContractRelationshipHandler(
        ILogger<CreateContractRelationshipHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.CreateContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateContractRelationshipHandler));

        // TODO:

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}