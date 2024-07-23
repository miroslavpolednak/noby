using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.GetIncomeList;

internal sealed class GetIncomeListHandler(
    IncomeMapper _incomeMapper,
    Database.HouseholdServiceDbContext _dbContext,
    IDocumentDataStorage _documentDataStorage,
    ILogger<GetIncomeListHandler> _logger)
        : IRequestHandler<Contracts.GetIncomeListRequest, Contracts.GetIncomeListResponse>
{
    public async Task<Contracts.GetIncomeListResponse> Handle(Contracts.GetIncomeListRequest request, CancellationToken cancellationToken)
    {
        var list = await _documentDataStorage.GetList<Database.DocumentDataEntities.Income, int>(request.CustomerOnSAId, cancellationToken);

        if (list.Count == 0 && !_dbContext.Customers.Any(t => t.CustomerOnSAId == request.CustomerOnSAId))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);
        }

        _logger.FoundItems(list.Count, nameof(Database.DocumentDataEntities.Income));

        var incomes = list.Select(t => _incomeMapper.MapFromDataToList(t));

        var response = new Contracts.GetIncomeListResponse();
        response.Incomes.AddRange(incomes);
        return response;
    }
}