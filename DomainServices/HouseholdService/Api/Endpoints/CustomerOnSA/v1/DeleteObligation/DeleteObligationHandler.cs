using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.DeleteObligation;

internal sealed class DeleteObligationHandler(
    IGrpcServerResponseCache _responseCache,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<DeleteObligationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Obligation, int>(request.ObligationId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);

        await _documentDataStorage.Delete<Database.DocumentDataEntities.Obligation>(request.ObligationId);

        await _responseCache.InvalidateEntry(nameof(GetCustomer), entity.EntityId);

        return new();
    }
}