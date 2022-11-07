using DomainServices.HouseholdService.Api.Repositories.Entities;
using _SA = DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal class CreateCustomerHandler
    : IRequestHandler<CreateCustomerMediatrRequest, _SA.CreateCustomerResponse>
{
    public async Task<_SA.CreateCustomerResponse> Handle(CreateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var model = new _SA.CreateCustomerResponse();

        // check existing SalesArrangementId
        await _salesArrangementService.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);

        var entity = new Repositories.Entities.CustomerOnSA
        {
            FirstNameNaturalPerson = request.Request.Customer.FirstNameNaturalPerson ?? "",
            Name = request.Request.Customer.Name ?? "",
            DateOfBirthNaturalPerson = request.Request.Customer.DateOfBirthNaturalPerson,
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId,
            LockedIncomeDateTime = request.Request.Customer?.LockedIncomeDateTime,
            MaritalStatusId = request.Request.Customer?.MaritalStatusId,
            Identities = request.Request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        bool containsKbIdentity = request.Request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb) ?? false;
        bool containsMpIdentity = request.Request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp) ?? false;

        // provolat sulm
        if (containsKbIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _sulmClient.StopUse(identity.IdentityId, "MPAP", cancellation);
            await _sulmClient.StartUse(identity.IdentityId, "MPAP", cancellation);
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (containsKbIdentity && !containsMpIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _updateService.GetCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellation);

            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity);
        }
        // nove byl customer identifikovan KB identitou
        else if (containsKbIdentity)
        {
            await _updateService.GetCustomerAndUpdateEntity(entity, entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb).IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb, cancellation);
        }

        // ulozit do DB
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
        model.CustomerOnSAId = entity.CustomerOnSAId;

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSA), entity.CustomerOnSAId);

        if (entity.Identities is not null)
            model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
                IdentityId = t.IdentityId
            }).ToList());

        return model;
    }

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClients _salesArrangementService;
    private readonly Shared.UpdateCustomerService _updateService;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClients salesArrangementService,
        SulmService.ISulmClient sulmClient,
        Shared.UpdateCustomerService updateService,
        Repositories.HouseholdServiceDbContext dbContext,
        ILogger<CreateCustomerHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
        _logger = logger;
    }
}