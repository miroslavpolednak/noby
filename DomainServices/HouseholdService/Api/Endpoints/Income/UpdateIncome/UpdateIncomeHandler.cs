using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.UpdateIncome;

internal sealed class UpdateIncomeHandler(
    IncomeMapper _incomeMapper,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<UpdateIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _incomeMapper.MapToData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        await _documentDataStorage.Update(request.IncomeId, documentEntity);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}