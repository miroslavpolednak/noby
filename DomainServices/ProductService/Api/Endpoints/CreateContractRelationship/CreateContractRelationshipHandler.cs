using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.ProductService.Api.Endpoints.CreateContractRelationship;

internal sealed class CreateContractRelationshipHandler(
	ICodebookServiceClient _codebookService,
	IMpHomeClient _mpHomeClient) 
    : IRequestHandler<CreateContractRelationshipRequest>
{
	public async Task Handle(CreateContractRelationshipRequest request, CancellationToken cancellation)
    {
		var relationshipTypeItem = await getContractRelationshipType(request.Relationship.ContractRelationshipTypeId);

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = parseRelationshipType(relationshipTypeItem.MpDigiApiCode)
        };

        await _mpHomeClient.UpdateLoanPartnerLink(request.ProductId, request.Relationship.PartnerId, loanLinkRequest, cancellation);
    }

    /// <summary>
    /// Returns RelationshipCustomerProductType codebook item by ID
    /// </summary>
    private async Task<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem> getContractRelationshipType(int contractRelationshipTypeId)
    {
        var list = await _codebookService.RelationshipCustomerProductTypes();

        return list.FirstOrDefault(t => t.Id == contractRelationshipTypeId)
               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12013, contractRelationshipTypeId);
    }

    private static ContractRelationshipType parseRelationshipType(string mpDigiApiCode)
    {
        if (Enum.TryParse(mpDigiApiCode, out ContractRelationshipType relationshipType))
            return relationshipType;

        throw new CisArgumentException(1,
                                       $"Value of RelationshipCustomerProductType.MpDigiApiCode [{mpDigiApiCode}] can´t be converted to MpHome.ContractRelationshipType",
                                       nameof(RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem));
    }
}