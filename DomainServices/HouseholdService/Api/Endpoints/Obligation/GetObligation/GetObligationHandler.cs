using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.GetObligation;

internal sealed class GetObligationHandler
    : IRequestHandler<GetObligationRequest, Contracts.Obligation>
{
    public async Task<Contracts.Obligation> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .Select(Database.CustomerOnSAServiceExpressions.Obligation())
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);

        return model;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetObligationHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}