using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA;

internal class DeleteIncomeHandler
    : IRequestHandler<Dto.DeleteIncomeMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteIncomeMediatrRequest request, CancellationToken cancellation)
    {
        //TODO kontrola zda muze smazat?

        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Income ID {request.IncomeId} does not exist.");
         
        _dbContext.CustomersIncomes.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<DeleteIncomeHandler> _logger;

    public DeleteIncomeHandler(
        Repositories.HouseholdServiceDbContext dbContext,
        ILogger<DeleteIncomeHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
