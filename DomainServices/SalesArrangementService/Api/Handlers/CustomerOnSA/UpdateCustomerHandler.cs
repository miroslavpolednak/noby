using Microsoft.EntityFrameworkCore;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateCustomerHandler
    : IRequestHandler<Dto.UpdateCustomerMediatrRequest, _SA.UpdateCustomerResponse>
{
    public async Task<_SA.UpdateCustomerResponse> Handle(Dto.UpdateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        // response instance
        var model = new _SA.UpdateCustomerResponse();

        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.Request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId} does not exist.");

        // helper aby se nemuselo porad null checkovat
        entity.Identities ??= new List<Repositories.Entities.CustomerOnSAIdentity>();

        // customerOnSA byl jiz updatovan z KB CM
        bool alreadyKbUpdatedCustomer = entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);

        // provolat sulm
        if (alreadyKbUpdatedCustomer)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _sulmClient.StopUse(identity.IdentityId, "MPAP");
            await _sulmClient.StartUse(identity.IdentityId, "MPAP");
        }

        // vychazim z toho, ze identitu klienta nelze menit. Tj. z muze prijit prazdna kolekce CustomerIdentifiers v requestu, ale to neznamena, ze jiz existujici identity na COnSA odstranim.
        if (request.Request.Customer.CustomerIdentifiers is not null && request.Request.Customer.CustomerIdentifiers.Any())
        {
            var existingSchemas = entity.Identities.Select(t => (int)t.IdentityScheme).ToList();
            var newSchemasToAdd = request.Request.Customer.CustomerIdentifiers.Where(t => !existingSchemas.Contains((int)t.IdentityScheme)).ToList();

            entity.Identities.AddRange(newSchemasToAdd.Select(t => new Repositories.Entities.CustomerOnSAIdentity(t, entity.CustomerOnSAId)));
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (!entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Mp)
            && entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb))
        {
            var identity = entity.Identities.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _updateService.GetCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellation);

            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity);

            // pokud probehlo zadani klienta v eas ok, tak pridej schema
            /*if (newMpIdentityId.HasValue)
            {
                model.PartnerId = newMpIdentityId.Value;
            }*/
        }
        // nove byl customer identifikovan KB identitou
        else if (!alreadyKbUpdatedCustomer && entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb))
        {
            await _updateService.GetCustomerAndUpdateEntity(entity, entity.Identities.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb).IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb, cancellation);
        }
        // customer zije zatim jen v NOBY, mohu updatovat maritalState
        else if (!alreadyKbUpdatedCustomer)
        {
            entity.MaritalStatusId = request.Request.Customer.MaritalStatusId;
        }

        // update CustomerOnSA
        entity.LockedIncomeDateTime = request.Request.Customer.LockedIncomeDateTime;

        await _dbContext.SaveChangesAsync(cancellation);

        model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
        {
            IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
            IdentityId = t.IdentityId
        }).ToList());

        return model;
    }

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly Shared.UpdateCustomerService _updateService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public UpdateCustomerHandler(
        SulmService.ISulmClient sulmClient,
        Shared.UpdateCustomerService updateService,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
    }
}