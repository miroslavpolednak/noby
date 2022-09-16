using Microsoft.EntityFrameworkCore;
using Google.Protobuf;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class DeleteCustomerHandler
    : IRequestHandler<Dto.DeleteCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // nemuze to byt hlavni dluznik
        if (entity.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor)
            throw new CisValidationException(16053, "CustomerOnSA is in role=Debtor -> can't be deleted");

        // SULM
        if (entity.Identities?.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb) ?? false)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _sulmClient.StopUse(identity.IdentityId, "MPAP");
        }

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saParameterInstance = await _dbContext.SalesArrangementsParameters
            .FirstOrDefaultAsync(t => t.SalesArrangementId == entity.SalesArrangementId, cancellation);
        if (saParameterInstance?.ParametersBin is not null)
        {
            var parameter = _SA.SalesArrangementParametersMortgage.Parser.ParseFrom(saParameterInstance.ParametersBin);
            if (parameter.Agent == request.CustomerOnSAId)
            {
                // ziskat caseId
                int? mainCustomerOnSAId = (await _dbContext
                    .Households
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.SalesArrangementId == entity.SalesArrangementId && t.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main, cancellation)
                    )?.CustomerOnSAId1;
                
                parameter.Agent = mainCustomerOnSAId;
                saParameterInstance.Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(parameter);
                saParameterInstance.ParametersBin = parameter.ToByteArray();
            }
        }

        _dbContext.Customers.Remove(entity);

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.CustomerOnSAIdentity WHERE CustomerOnSAId={request.CustomerOnSAId}", cancellation);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public DeleteCustomerHandler(
        SulmService.ISulmClient sulmClient,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _sulmClient = sulmClient;
        _dbContext = dbContext;
    }
}