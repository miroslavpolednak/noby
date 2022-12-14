using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomer;

internal sealed class GetCustomerHandler
    : IRequestHandler<GetCustomerRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        var customerInstance = new Contracts.CustomerOnSA
        {
            CustomerOnSAId = entity.CustomerOnSAId,
            Name = entity.Name,
            FirstNameNaturalPerson = entity.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = entity.DateOfBirthNaturalPerson,
            SalesArrangementId = entity.SalesArrangementId,
            CustomerRoleId = (int)entity.CustomerRoleId,
            LockedIncomeDateTime = entity.LockedIncomeDateTime,
            MaritalStatusId = entity.MaritalStatusId
        };

        if (entity.AdditionalDataBin != null)
            customerInstance.CustomerAdditionalData = CustomerAdditionalData.Parser.ParseFrom(entity.AdditionalDataBin);
        if (entity.ChangeDataBin != null)
        {
            var arr = CustomerChangeDataItemWrapper.Parser.ParseFrom(entity.ChangeDataBin).ChangeData;
            customerInstance.CustomerChangeData.AddRange(arr);
        }
        
        // identity
        var identities = await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        customerInstance.CustomerIdentifiers.AddRange(identities.Select(t => new Identity(t.IdentityId, t.IdentityScheme)));

        // obligations
        var obligations = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Obligation())
            .ToListAsync(cancellationToken);
        customerInstance.Obligations.AddRange(obligations);

        // incomes
        var list = await _dbContext.CustomersIncomes
           .AsNoTracking()
           .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
           .Select(CustomerOnSAServiceExpressions.Income())
           .ToListAsync(cancellationToken);
        customerInstance.Incomes.AddRange(list);

        return customerInstance;
    }

    private readonly HouseholdServiceDbContext _dbContext;

    public GetCustomerHandler(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}