using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.DeleteObligation;

internal sealed class DeleteObligationHandler(IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<DeleteObligationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        var deletedRows = await _documentDataStorage.Delete<Database.DocumentDataEntities.Obligation>(request.ObligationId);

        if (deletedRows == 0)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}