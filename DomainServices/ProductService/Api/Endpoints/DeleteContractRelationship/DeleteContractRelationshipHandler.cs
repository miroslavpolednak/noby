namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipHandler : IRequestHandler<DeleteContractRelationshipRequest>
{
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public DeleteContractRelationshipHandler(
        LoanRepository repository,
        IMpHomeClient mpHomeClient)
    {
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }

    public async Task Handle(DeleteContractRelationshipRequest request, CancellationToken cancellationToken)
    {
        // check if loan exists (against KonsDB)
        if (!await _repository.LoanExists(request.ProductId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // check if relationship exists
        if (!await _repository.RelationshipExists(request.ProductId, request.PartnerId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12018, request.ProductId, request.PartnerId);

        // call endpoint
        await _mpHomeClient.DeletePartnerLoanLink(request.ProductId, request.PartnerId, cancellationToken);
    }

}