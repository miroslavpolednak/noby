using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Handlers;

internal class DeleteContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Dto.DeleteContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public DeleteContractRelationshipHandler(
        ICodebookServiceClients codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<DeleteContractRelationshipHandler> logger) : base(codebookService, repository, mpHomeClient, logger)
    { }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        // check if relationship exists
        var relationshipExists = await _repository.ExistsRelationship(request.Request.ProductId, request.Request.PartnerId, cancellation);
        if (!relationshipExists)
        {
            throw new CisNotFoundException(12018,
                $"{nameof(Repositories.Entities.Relationship)} with ProductId {request.Request.ProductId} and PartnerId {request.Request.PartnerId} does not exist.");
        }

        // call endpoint
        await _mpHomeClient.DeletePartnerLoanLink(request.Request.ProductId, request.Request.PartnerId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}