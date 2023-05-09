using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Contracts.DeleteContractRelationshipRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public DeleteContractRelationshipHandler(
        ICodebookServiceClients codebookService,
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<DeleteContractRelationshipHandler> logger) : base(codebookService, repository, mpHomeClient, logger)
    { }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.DeleteContractRelationshipRequest request, CancellationToken cancellation)
    {
        // check if relationship exists
        if (!await _repository.ExistsRelationship(request.ProductId, request.PartnerId, cancellation))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12018, request.ProductId, request.PartnerId);
        }

        // call endpoint
        await _mpHomeClient.DeletePartnerLoanLink(request.ProductId, request.PartnerId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

}