﻿using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints.DeleteContractRelationship;

internal sealed class DeleteContractRelationshipHandler
    : IRequestHandler<Contracts.DeleteContractRelationshipRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction
    private readonly Database.LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public DeleteContractRelationshipHandler(
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient)
    {
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.DeleteContractRelationshipRequest request, CancellationToken cancellation)
    {
        // check if relationship exists
        var relationshipExists = await _repository.ExistsRelationship(request.ProductId, request.PartnerId, cancellation);
        if (!relationshipExists)
        {
            throw new CisNotFoundException(12018,
                $"{nameof(Database.Entities.Relationship)} with ProductId {request.ProductId} and PartnerId {request.PartnerId} does not exist.");
        }

        // call endpoint
        await _mpHomeClient.DeletePartnerLoanLink(request.ProductId, request.PartnerId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

}