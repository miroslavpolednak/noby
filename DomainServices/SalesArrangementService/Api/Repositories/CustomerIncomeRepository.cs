namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CustomerIncomeRepository
{


    private readonly SalesArrangementServiceDbContext _dbContext;

    public CustomerIncomeRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
