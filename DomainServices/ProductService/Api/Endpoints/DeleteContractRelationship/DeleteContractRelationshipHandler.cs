namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipHandler(IMpHomeClient _mpHomeClient) 
    : IRequestHandler<DeleteContractRelationshipRequest>
{
	public async Task Handle(DeleteContractRelationshipRequest request, CancellationToken cancellationToken)
    {
        // call endpoint
        await _mpHomeClient.DeletePartnerLoanLink(request.ProductId, request.PartnerId, cancellationToken);
    }
}