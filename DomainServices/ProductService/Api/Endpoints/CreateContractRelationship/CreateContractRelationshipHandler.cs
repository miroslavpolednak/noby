using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;
using ExternalServices.MpHome.V1_1;
using ExternalServices.MpHome.V1_1.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.CreateContractRelationship;

internal sealed class CreateContractRelationshipHandler
    : IRequestHandler<Contracts.CreateContractRelationshipRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction
    private readonly ICodebookServiceClients _codebookService;
    private readonly Database.LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public CreateContractRelationshipHandler(
        ICodebookServiceClients codebookService,
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient)
    {
        _codebookService = codebookService;
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.CreateContractRelationshipRequest request, CancellationToken cancellation)
    {
        // check if relationship not exists
        var relationshipExists = await _repository.ExistsRelationship(request.ProductId, request.Relationship.PartnerId, cancellation);
        if (relationshipExists)
        {
            throw new CisAlreadyExistsException(12011,
                $"{nameof(Database.Entities.Relationship)} with ProductId {request.ProductId} and PartnerId {request.Relationship.PartnerId} already exists");
        }

        // check if loan exists (against KonsDB)
        var loanExists = await _repository.ExistsLoan(request.ProductId, cancellation);
        if (!loanExists)
        {
            throw new CisNotFoundException(12001, nameof(Database.Entities.Loan), request.ProductId);
        }

        // check if partner exists (against KonsDB)
        var partnerExists = await _repository.ExistsPartner(request.Relationship.PartnerId, cancellation);
        if (!partnerExists)
        {
            throw new CisNotFoundException(12012, nameof(Database.Entities.Partner), request.Relationship.PartnerId);
        }

        // get codebook RelationshipCustomerProductTypeItem item
        var relationshipTypeItem = await GetContractRelationshipType(request.Relationship.ContractRelationshipTypeId);

        // get MpHome.ContractRelationshipType by MpDigiApiCode
        if (!Enum.TryParse(relationshipTypeItem.MpDigiApiCode, out ContractRelationshipType type))
            throw new CisArgumentException(1, $"Value of RelationshipCustomerProductType.MpDigiApiCode [{relationshipTypeItem.MpDigiApiCode}] can´t be converted to MpHome.ContractRelationshipType", nameof(RelationshipCustomerProductTypeItem));

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = type
        };

        // call endpoint
        await _mpHomeClient.UpdateLoanPartnerLink(request.ProductId, request.Relationship.PartnerId, loanLinkRequest, cancellation);

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