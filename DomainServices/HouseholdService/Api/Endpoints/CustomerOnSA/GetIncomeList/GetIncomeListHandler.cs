using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncomeList;

internal sealed class GetIncomeListHandler
    : IRequestHandler<GetIncomeListRequest, Contracts.GetIncomeListResponse>
{
    public async Task<Contracts.GetIncomeListResponse> Handle(GetIncomeListRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.CustomersIncomes
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Income())
            .ToListAsync(cancellationToken);

        _logger.FoundItems(list.Count, nameof(Database.Entities.CustomerOnSAIncome));

        var response = new Contracts.GetIncomeListResponse();
        response.Incomes.AddRange(list);
        return response;
    }

    private readonly HouseholdServiceDbContext _dbContext;
    private readonly ILogger<GetIncomeListHandler> _logger;

    public GetIncomeListHandler(
        HouseholdServiceDbContext dbContext,
        ILogger<GetIncomeListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
