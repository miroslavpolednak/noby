using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.DeleteIncome;

internal sealed class DeleteIncomeHandler(
    IGrpcServerResponseCache _responseCache,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<DeleteIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        var entity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Income, int>(request.IncomeId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);

        await _documentDataStorage.Delete<Database.DocumentDataEntities.Income>(request.IncomeId);

        await _responseCache.InvalidateEntry(nameof(GetCustomer), entity.EntityId);

        return new();
    }
}