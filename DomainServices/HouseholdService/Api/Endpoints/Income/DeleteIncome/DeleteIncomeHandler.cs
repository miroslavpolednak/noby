using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.DeleteIncome;

internal sealed class DeleteIncomeHandler
    : IRequestHandler<DeleteIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        var deletedRows = await _dbContext
            .CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public DeleteIncomeHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}