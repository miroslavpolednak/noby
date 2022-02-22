using ExternalServices.MpHome.V1._1;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.ProductService.Api.Handlers;

internal class DeleteContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Dto.DeleteContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public DeleteContractRelationshipHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<DeleteContractRelationshipHandler> logger) : base(codebookService, repository, mpHomeClient, logger)
    { }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(DeleteContractRelationshipHandler));

        // check if relationship exists
        var relationshipExists = await _repository.ExistsRelationship(request.Request.ProductId, request.Request.PartnerId, cancellation);
        if (!relationshipExists)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Relationship)); //TODO: error code
        }

        // call endpoint
        ServiceCallResult.Resolve(await _mpHomeClient.DeletePartnerLoanLink(request.Request.ProductId, request.Request.PartnerId));

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}