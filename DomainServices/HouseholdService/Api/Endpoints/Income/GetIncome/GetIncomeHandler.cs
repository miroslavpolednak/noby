using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.GetIncome;

internal sealed class GetIncomeHandler(
    IDocumentDataStorage _documentDataStorage, 
    IncomeMapper _incomeMapper)
        : IRequestHandler<GetIncomeRequest, Contracts.Income>
{
    public async Task<Contracts.Income> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Income, int>(request.IncomeId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);

        var model = _incomeMapper.MapFromDataToSingle(documentEntity.Data!);

        model.IncomeId = documentEntity.DocumentDataStorageId;
        model.CustomerOnSAId = documentEntity.EntityId;

        return model;
    }
}