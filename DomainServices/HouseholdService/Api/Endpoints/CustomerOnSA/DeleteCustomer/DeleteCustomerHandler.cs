using DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal sealed class DeleteCustomerHandler
    : IRequestHandler<DeleteCustomerRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // nemuze to byt hlavni dluznik
        if (entity.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor && !request.HardDelete)
            throw new CisValidationException(16053, "CustomerOnSA is in role=Debtor -> can't be deleted");

        var kbIdentity = entity.Identities?.FirstOrDefault(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);

        // smazat prijmy
        await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAId == entity.CustomerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);
        // smazat zavazky
        await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAId == entity.CustomerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);
        // smazat customera
        _dbContext.Customers.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // SULM
        if (kbIdentity is not null)
        {
            await _sulmClient.StopUse(kbIdentity.IdentityId, "MPAP", cancellationToken);
        }

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saInstance = await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellationToken);
        if (saInstance.Mortgage?.Agent == request.CustomerOnSAId)
        {
            await deleteAgentFromSalesArrangement(saInstance, entity.SalesArrangementId, cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task deleteAgentFromSalesArrangement(_SA.SalesArrangement saInstance, int salesArrangementId, CancellationToken cancellationToken)
    {
        // ziskat ID hlavniho customera
        int? mainCustomerOnSAId = (await _dbContext
            .Households
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId && t.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main, cancellationToken)
        )?.CustomerOnSAId1;
        saInstance.Mortgage.Agent = mainCustomerOnSAId;

        await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangementId,
            Mortgage = saInstance.Mortgage
        }, cancellationToken);
    }

    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly SulmService.ISulmClient _sulmClient;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public DeleteCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        SulmService.ISulmClient sulmClient,
        Database.HouseholdServiceDbContext dbContext)
    {
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _dbContext = dbContext;
    }
}