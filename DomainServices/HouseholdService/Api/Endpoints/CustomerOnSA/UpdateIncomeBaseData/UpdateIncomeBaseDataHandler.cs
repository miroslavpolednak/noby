namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal class UpdateIncomeBaseDataHandler
    : IRequestHandler<UpdateIncomeBaseDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeBaseDataMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");

        // base data
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;

        await _dbContext.SaveChangesAsync(cancellation);

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
