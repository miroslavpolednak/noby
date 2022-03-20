using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class GetIncomeListHandler
    : IRequestHandler<Dto.GetIncomeListMediatrRequest, Contracts.GetIncomeListResponse>
{
    public async Task<Contracts.GetIncomeListResponse> Handle(Dto.GetIncomeListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncomeListHandler), request.CustomerOnSAId);

        var list = await _dbContext.CustomersIncomes
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => new Contracts.IncomeInList
            {
                IncomeId = t.CustomerIncomeId,
                IncomeTypeId = (int)t.IncomeTypeId,
                CurrencyId = t.CurrencyId,
                Sum = t.Sum
            })
            .ToListAsync(cancellation);

        _logger.FoundItems(list.Count, nameof(Repositories.Entities.CustomerIncome));

        var response = new Contracts.GetIncomeListResponse();
        response.Incomes.AddRange(list);
        return response;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<GetIncomeListHandler> _logger;

    public GetIncomeListHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<GetIncomeListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
