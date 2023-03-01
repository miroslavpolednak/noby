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
            .Include(t => t.Identities)
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken) 
            ?? throw new CisNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, ErrorCodeMapper.GetMessage(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId));

        var customerInstance = new Contracts.CustomerOnSA
        {
            CustomerOnSAId = entity.CustomerOnSAId,
            Name = entity.Name,
            FirstNameNaturalPerson = entity.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = entity.DateOfBirthNaturalPerson,
            SalesArrangementId = entity.SalesArrangementId,
            CustomerRoleId = (int)entity.CustomerRoleId,
            LockedIncomeDateTime = entity.LockedIncomeDateTime,
            MaritalStatusId = entity.MaritalStatusId,
            CustomerChangeData = entity.ChangeData
        };

        if (entity.AdditionalDataBin != null)
            customerInstance.CustomerAdditionalData = CustomerAdditionalData.Parser.ParseFrom(entity.AdditionalDataBin);
        
        // identity
        if (entity.Identities is not null)
            customerInstance.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new Identity(t.IdentityId, t.IdentityScheme)));

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