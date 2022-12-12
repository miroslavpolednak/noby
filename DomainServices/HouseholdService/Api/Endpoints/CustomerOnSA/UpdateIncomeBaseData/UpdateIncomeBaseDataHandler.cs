using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal class UpdateIncomeBaseDataHandler
    : IRequestHandler<UpdateIncomeBaseDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeBaseDataRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16029, $"Income ID {request.IncomeId} does not exist.");

        // base data
        entity.Sum = request.BaseData?.Sum;
        entity.CurrencyCode = request.BaseData?.CurrencyCode;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<UpdateIncomeBaseDataHandler> _logger;

    public UpdateIncomeBaseDataHandler(
        Database.HouseholdServiceDbContext dbContext,
        ILogger<UpdateIncomeBaseDataHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
