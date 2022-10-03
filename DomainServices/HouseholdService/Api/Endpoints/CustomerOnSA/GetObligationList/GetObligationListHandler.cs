using DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncomeList;
using DomainServices.HouseholdService.Api.Repositories;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligationList;

internal sealed class GetObligationListHandler
    : IRequestHandler<GetObligationListMediatrRequest, Contracts.GetObligationListResponse>
{
    public async Task<Contracts.GetObligationListResponse> Handle(GetObligationListMediatrRequest request, CancellationToken cancellation)
    {
        var list = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Obligation())
            .ToListAsync(cancellation);

        _logger.FoundItems(list.Count, nameof(Repositories.Entities.CustomerOnSAIncome));

        var response = new Contracts.GetObligationListResponse();
        response.Obligations.AddRange(list);
        return response;
    }

    private readonly HouseholdServiceDbContext _dbContext;
    private readonly ILogger<GetIncomeListHandler> _logger;

    public GetObligationListHandler(
        HouseholdServiceDbContext dbContext,
        ILogger<GetIncomeListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
