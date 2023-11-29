using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.GetIncome;

internal sealed class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, Contracts.Income>
{
    public async Task<Contracts.Income> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Income>(request.IncomeId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);

        var model = _incomeMapper.MapDataToSingle(documentEntity.Data!);

        model.IncomeId = documentEntity.DocumentDataStorageId;
        model.CustomerOnSAId = documentEntity.EntityIdInt;

        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Services.IncomeFromDataMapper _incomeMapper;

    public GetIncomeHandler(IDocumentDataStorage documentDataStorage, Services.IncomeFromDataMapper incomeMapper)
    {
        _documentDataStorage = documentDataStorage;
        _incomeMapper = incomeMapper;
    }
}