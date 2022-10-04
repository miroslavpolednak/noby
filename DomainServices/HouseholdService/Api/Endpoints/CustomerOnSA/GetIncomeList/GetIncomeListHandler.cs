using DomainServices.HouseholdService.Api.Repositories;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncomeList;

internal sealed class GetIncomeListHandler
    : IRequestHandler<GetIncomeListMediatrRequest, Contracts.GetIncomeListResponse>
{
    public async Task<Contracts.GetIncomeListResponse> Handle(GetIncomeListMediatrRequest request, CancellationToken cancellation)
    {
        var list = await _dbContext.CustomersIncomes
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Income())
            .ToListAsync(cancellation);

        _logger.FoundItems(list.Count, nameof(Repositories.Entities.CustomerOnSAIncome));

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
