using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using System.Diagnostics.CodeAnalysis;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateCustomerHandler
    : IRequestHandler<Dto.CreateCustomerMediatrRequest, _SA.CreateCustomerResponse>
{
    public async Task<_SA.CreateCustomerResponse> Handle(Dto.CreateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var model = new _SA.CreateCustomerResponse();

        // check existing SalesArrangementId
        await _saRepository.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);

        var entity = new Repositories.Entities.CustomerOnSA
        {
            FirstNameNaturalPerson = request.Request.Customer.FirstNameNaturalPerson ?? "",
            Name = request.Request.Customer.Name ?? "",
            DateOfBirthNaturalPerson = request.Request.Customer.DateOfBirthNaturalPerson,
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId,
            LockedIncomeDateTime = request.Request.Customer.LockedIncomeDateTime,
            Identities = request.Request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        bool containsKbIdentity = request.Request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb) ?? false;
        bool containsMpIdentity = request.Request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp) ?? false;

        // uz ma KB identitu, ale jeste nema MP identitu
        if (containsKbIdentity && !containsMpIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _updateService.GetCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellation);

            // zavolat EAS
            int? newMpIdentityId = await _updateService.TryCreateMpIdentity(entity);

            // pokud probehlo zadani klienta v eas ok, tak pridej schema
            if (newMpIdentityId.HasValue)
            {
                model.PartnerId = newMpIdentityId.Value;
            }
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

    private readonly Shared.UpdateCustomerService _updateService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    
    public CreateCustomerHandler(
        Shared.UpdateCustomerService updateService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateCustomerHandler> logger)
    {
        _updateService = updateService;
        _dbContext = dbContext;
        _saRepository = saRepository;
        _logger = logger;
    }
}