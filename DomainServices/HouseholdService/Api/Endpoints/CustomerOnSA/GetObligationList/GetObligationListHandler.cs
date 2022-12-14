using DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncomeList;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligationList;

internal sealed class GetObligationListHandler
    : IRequestHandler<GetObligationListRequest, GetObligationListResponse>
{
    public async Task<GetObligationListResponse> Handle(GetObligationListRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Obligation())
            .ToListAsync(cancellationToken);

        _logger.FoundItems(list.Count, nameof(Database.Entities.CustomerOnSAIncome));

        var response = new GetObligationListResponse();
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
