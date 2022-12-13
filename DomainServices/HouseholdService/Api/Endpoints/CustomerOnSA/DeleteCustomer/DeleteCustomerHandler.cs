﻿using DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal class DeleteCustomerHandler
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

        // smazat customera
        _dbContext.Customers.Remove(entity);

        // v EF7 zmenit na nativni delete
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(@$"
DELETE FROM dbo.CustomerOnSAIncome WHERE CustomerOnSAId={entity.CustomerOnSAId};
DELETE FROM dbo.CustomerOnSAObligation WHERE CustomerOnSAId={entity.CustomerOnSAId}", cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // SULM
        if (kbIdentity is not null)
        {
            await _sulmClient.StopUse(kbIdentity.IdentityId, "MPAP", cancellationToken);
        }

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellationToken));
        if (saInstance.Mortgage?.Agent == request.CustomerOnSAId)
        {
            // ziskat ID hlavniho customera
            int? mainCustomerOnSAId = (await _dbContext
                .Households
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.SalesArrangementId == entity.SalesArrangementId && t.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main, cancellationToken)
                )?.CustomerOnSAId1;
            saInstance.Mortgage.Agent = mainCustomerOnSAId;

            await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
            {
                SalesArrangementId = entity.SalesArrangementId,
                Mortgage = saInstance.Mortgage
            }, cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
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