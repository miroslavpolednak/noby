using DomainServices.HouseholdService.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA;

internal class GetObligationListHandler
    : IRequestHandler<Dto.GetObligationListMediatrRequest, Contracts.GetObligationListResponse>
{
    public async Task<Contracts.GetObligationListResponse> Handle(Dto.GetObligationListMediatrRequest request, CancellationToken cancellation)
    {
        var list = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceRepositoryExpressions.Obligation())
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
