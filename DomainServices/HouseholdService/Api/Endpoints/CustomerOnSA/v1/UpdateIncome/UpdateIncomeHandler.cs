using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateIncome;

internal sealed class UpdateIncomeHandler(
    IGrpcServerResponseCache _responseCache,
    IncomeMapper _incomeMapper,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<Income, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Income request, CancellationToken cancellationToken)
    {
        var documentEntity = await _incomeMapper.MapToData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        await _documentDataStorage.Update(request.IncomeId, documentEntity);

        await _responseCache.InvalidateEntry(nameof(GetCustomer), request.CustomerOnSAId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}