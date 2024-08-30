using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.DeleteObligation;

internal sealed class DeleteObligationHandler(IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<DeleteObligationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        int count = await _documentDataStorage.Delete<Database.DocumentDataEntities.Obligation>(request.ObligationId);

        if (count == 0)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);
        }

        return new();
    }
}