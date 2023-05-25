using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using ExternalServices.MpHome.V1;
using ExternalServices.MpHome.V1.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.CreateContractRelationship;

internal sealed class CreateContractRelationshipHandler
    : IRequestHandler<Contracts.CreateContractRelationshipRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction
    private readonly ICodebookServiceClient _codebookService;
    private readonly Database.LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public CreateContractRelationshipHandler(
        ICodebookServiceClient codebookService,
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
        if (await _repository.ExistsRelationship(request.ProductId, request.Relationship.PartnerId, cancellation))
        {
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.AlreadyExists12011, request.ProductId, request.Relationship.PartnerId); 
        }

        // check if loan exists (against KonsDB)
        if (!await _repository.ExistsLoan(request.ProductId, cancellation))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);
        }

        // check if partner exists (against KonsDB)
        if (!await _repository.ExistsPartner(request.Relationship.PartnerId, cancellation))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12012, request.Relationship.PartnerId);
        }

        // get codebook RelationshipCustomerProductTypeItem item
        var relationshipTypeItem = await GetContractRelationshipType(request.Relationship.ContractRelationshipTypeId);

        // get MpHome.ContractRelationshipType by MpDigiApiCode
        if (!Enum.TryParse(relationshipTypeItem.MpDigiApiCode, out ContractRelationshipType type))
            throw new CisArgumentException(1, $"Value of RelationshipCustomerProductType.MpDigiApiCode [{relationshipTypeItem.MpDigiApiCode}] can´t be converted to MpHome.ContractRelationshipType", nameof(RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem));

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
    private async Task<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem> GetContractRelationshipType(int contractRelationshipTypeId)
    {
        var list = await _codebookService.RelationshipCustomerProductTypes();
        var item = list.FirstOrDefault(t => t.Id == contractRelationshipTypeId);

        if (item == null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12013, contractRelationshipTypeId);
        }

        return item;
    }
}