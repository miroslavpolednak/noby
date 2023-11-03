using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.ProductService.Api.Endpoints.CreateContractRelationship;

internal sealed class CreateContractRelationshipHandler : IRequestHandler<CreateContractRelationshipRequest>
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public CreateContractRelationshipHandler(
        ICodebookServiceClient codebookService,
        LoanRepository repository,
        IMpHomeClient mpHomeClient)
    {
        _codebookService = codebookService;
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }

    public async Task Handle(CreateContractRelationshipRequest request, CancellationToken cancellation)
    {
        // check if relationship already exists
        if (await _repository.RelationshipExists(request.ProductId, request.Relationship.PartnerId, cancellation))
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.AlreadyExists12011, request.ProductId, request.Relationship.PartnerId);

        // check if loan exists (against KonsDB)
        if (!await _repository.LoanExists(request.ProductId, cancellation))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // check if partner exists (against KonsDB)
        if (!await _repository.PartnerExists(request.Relationship.PartnerId, cancellation))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12012, request.Relationship.PartnerId);

        var relationshipTypeItem = await GetContractRelationshipType(request.Relationship.ContractRelationshipTypeId);

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = ParseRelationshipType(relationshipTypeItem.MpDigiApiCode)
        };

        await _mpHomeClient.UpdateLoanPartnerLink(request.ProductId, request.Relationship.PartnerId, loanLinkRequest, cancellation);
    }

    /// <summary>
    /// Returns RelationshipCustomerProductType codebook item by ID
    /// </summary>
    private async Task<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem> GetContractRelationshipType(int contractRelationshipTypeId)
    {
        var list = await _codebookService.RelationshipCustomerProductTypes();

        return list.FirstOrDefault(t => t.Id == contractRelationshipTypeId)
               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12013, contractRelationshipTypeId);
    }

    private ContractRelationshipType ParseRelationshipType(string mpDigiApiCode)
    {
        if (Enum.TryParse(mpDigiApiCode, out ContractRelationshipType relationshipType))
            return relationshipType;

        throw new CisArgumentException(1,
                                       $"Value of RelationshipCustomerProductType.MpDigiApiCode [{mpDigiApiCode}] can´t be converted to MpHome.ContractRelationshipType",
                                       nameof(RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem));
    }
}