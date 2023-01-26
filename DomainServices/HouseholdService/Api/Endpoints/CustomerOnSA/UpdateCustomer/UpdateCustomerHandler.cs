using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomer;

internal sealed class UpdateCustomerHandler
    : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        // response instance
        var model = new UpdateCustomerResponse();

        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // helper aby se nemuselo porad null checkovat
        entity.Identities ??= new List<Database.Entities.CustomerOnSAIdentity>();

        // customerOnSA byl jiz updatovan z KB CM
        bool alreadyKbUpdatedCustomer = entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);

        // vychazim z toho, ze identitu klienta nelze menit. Tj. z muze prijit prazdna kolekce CustomerIdentifiers v requestu, ale to neznamena, ze jiz existujici identity na COnSA odstranim.
        if (request.Customer.CustomerIdentifiers is not null && request.Customer.CustomerIdentifiers.Any())
        {
            var existingSchemas = entity.Identities.Select(t => (int)t.IdentityScheme).ToList();
            var newSchemasToAdd = request.Customer.CustomerIdentifiers.Where(t => !existingSchemas.Contains((int)t.IdentityScheme)).ToList();

            entity.Identities.AddRange(newSchemasToAdd.Select(t => new Database.Entities.CustomerOnSAIdentity(t, entity.CustomerOnSAId)));
        }

        // provolat sulm - pokud jiz ma nebo mu byla akorat pridana KB identita
        var kbIdentity = entity.Identities.FirstOrDefault(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
        if (kbIdentity is not null)
        {
            await _sulmClient.StopUse(kbIdentity.IdentityId, "MPAP", cancellationToken);
            await _sulmClient.StartUse(kbIdentity.IdentityId, "MPAP", cancellationToken);
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (!entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Mp) && kbIdentity is not null)
        {
            await _updateService.GetCustomerAndUpdateEntity(entity, kbIdentity.IdentityId, kbIdentity.IdentityScheme, cancellationToken);

            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity, cancellationToken);
        }
        // nove byl customer identifikovan KB identitou
        else if (!alreadyKbUpdatedCustomer && kbIdentity is not null)
        {
            await _updateService.GetCustomerAndUpdateEntity(entity, kbIdentity.IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb, cancellationToken);
        }
        // customer zije zatim jen v NOBY, mohu updatovat maritalState
        else if (!alreadyKbUpdatedCustomer)
        {
            entity.MaritalStatusId = request.Customer.MaritalStatusId;
        }

        // update CustomerOnSA
        entity.LockedIncomeDateTime = request.Customer.LockedIncomeDateTime;

        await _dbContext.SaveChangesAsync(cancellationToken);

        model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
        {
            IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
            IdentityId = t.IdentityId
        }).ToList());

        return model;
    }

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly UpdateCustomerService _updateService;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateCustomerHandler(
        SulmService.ISulmClient sulmClient,
        UpdateCustomerService updateService,
        Database.HouseholdServiceDbContext dbContext)
    {
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
    }
}