using ExternalServices.MpHome.V1._1;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;
using DomainServices.CodebookService.Clients;

using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Dto.CreateContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public CreateContractRelationshipHandler(
        ICodebookServiceClients codebookService,
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
            throw new CisAlreadyExistsException(12011,
                $"{nameof(Repositories.Entities.Relationship)} with ProductId {request.Request.ProductId} and PartnerId {request.Request.Relationship.PartnerId} already exists");
        }

        // check if loan exists (against KonsDB)
        var loanExists = await _repository.ExistsLoan(request.Request.ProductId, cancellation);
        if (!loanExists)
        {
            throw new CisNotFoundException(12001, nameof(Repositories.Entities.Loan), request.Request.ProductId);
        }

        // check if partner exists (against KonsDB)
        var partnerExists = await _repository.ExistsPartner(request.Request.Relationship.PartnerId, cancellation);
        if (!partnerExists)
        {
            throw new CisNotFoundException(12012, nameof(Repositories.Entities.Partner), request.Request.Relationship.PartnerId);
        }

        // get codebook RelationshipCustomerProductTypeItem item
        var relationshipTypeItem = await GetContractRelationshipType(request.Request.Relationship.ContractRelationshipTypeId);

        // get MpHome.ContractRelationshipType by MpDigiApiCode
        if (!Enum.TryParse(relationshipTypeItem.MpDigiApiCode, out ContractRelationshipType type))
            throw new CisArgumentException(1, $"Value of RelationshipCustomerProductType.MpDigiApiCode [{relationshipTypeItem.MpDigiApiCode}] can´t be converted to MpHome.ContractRelationshipType", nameof(RelationshipCustomerProductTypeItem));

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = type
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
            throw new CisNotFoundException(12013, nameof(RelationshipCustomerProductTypeItem), contractRelationshipTypeId);
        }

        return item;
    }
}