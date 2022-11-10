using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal class DeleteCustomerHandler
    : IRequestHandler<DeleteCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // nemuze to byt hlavni dluznik
        if (entity.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor)
            throw new CisValidationException(16053, "CustomerOnSA is in role=Debtor -> can't be deleted");

        var kbIdentity = entity.Identities?.FirstOrDefault(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);

        // smazat customera
        _dbContext.Customers.Remove(entity);

        // v EF7 zmenit na nativni delete
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(@$"
DELETE FROM dbo.CustomerOnSAIncome WHERE CustomerOnSAId={entity.CustomerOnSAId};
DELETE FROM dbo.CustomerOnSAObligation WHERE CustomerOnSAId={entity.CustomerOnSAId}", cancellation);

        await _dbContext.SaveChangesAsync(cancellation);

        // SULM
        if (kbIdentity is not null)
        {
            await _sulmClient.StopUse(kbIdentity.IdentityId, "MPAP", cancellation);
        }

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellation));
        if (saInstance.Mortgage?.Agent == request.CustomerOnSAId)
        {
            // ziskat ID hlavniho customera
            int? mainCustomerOnSAId = (await _dbContext
                .Households
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.SalesArrangementId == entity.SalesArrangementId && t.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main, cancellation)
                )?.CustomerOnSAId1;
            saInstance.Mortgage.Agent = mainCustomerOnSAId;

            await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
            {
                SalesArrangementId = entity.SalesArrangementId,
                Mortgage = saInstance.Mortgage
            }, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClients _salesArrangementService;
    private readonly SulmService.ISulmClient _sulmClient;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public DeleteCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClients salesArrangementService,
        SulmService.ISulmClient sulmClient,
        Repositories.HouseholdServiceDbContext dbContext)
    {
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _dbContext = dbContext;
    }
}