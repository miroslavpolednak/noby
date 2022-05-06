using ExternalServices.MpHome.V1._1;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;
using DomainServices.CodebookService.Abstraction;

using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Dto.CreateContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public CreateContractRelationshipHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateContractRelationshipHandler> logger) : base(codebookService, repository, mpHomeClient, logger)
    {}

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.CreateContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        // check if relationship not exists
        var relationshipExists = await _repository.ExistsRelationship(request.Request.ProductId, request.Request.Relationship.PartnerId, cancellation);
        if (relationshipExists)
        {
            throw new CisAlreadyExistsException(13015, nameof(Repositories.Entities.Relationship)); //TODO: error code
        }

        // check if loan exists (against KonsDB)
        var loanExists = await _repository.ExistsLoan(request.Request.ProductId, cancellation);
        if (!loanExists)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Loan), request.Request.ProductId); //TODO: error code
        }

        // check if partner exists (against KonsDB)
        var partnerExists = await _repository.ExistsPartner(request.Request.Relationship.PartnerId, cancellation);
        if (!partnerExists)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Partner), request.Request.Relationship.PartnerId); //TODO: error code
        }

        // get codebook RelationshipCustomerProductTypeItem item
        var relationshipTypeItem = await GetContractRelationshipType(request.Request.Relationship.ContractRelationshipTypeId);

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = (ContractRelationshipType)relationshipTypeItem.MpHomeContractRelationshipType
        };

        // call endpoint
        ServiceCallResult.Resolve(await _mpHomeClient.UpdateLoanPartnerLink(request.Request.ProductId, request.Request.Relationship.PartnerId, loanLinkRequest));

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    /// <summary>
    /// Returns RelationshipCustomerProductType codebook item by ID
    /// </summary>
    private async Task<RelationshipCustomerProductTypeItem> GetContractRelationshipType(int contractRelationshipTypeId)
    {
        var list = await _codebookService.RelationshipCustomerProductTypes();
        var item = list.FirstOrDefault(t => t.Id == contractRelationshipTypeId);

        if (item == null)
        {
            throw new CisNotFoundException(13014, nameof(RelationshipCustomerProductTypeItem), contractRelationshipTypeId);
        }

        return item;
    }

}