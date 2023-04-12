using DomainServices.HouseholdService.Api.Database.Entities;
using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;
using Google.Protobuf;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal sealed class CreateCustomerHandler
    : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = new CreateCustomerResponse();

        // check existing SalesArrangementId
        await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var entity = new Database.Entities.CustomerOnSA
        {
            FirstNameNaturalPerson = request.Customer?.FirstNameNaturalPerson ?? "",
            Name = request.Customer?.Name ?? "",
            DateOfBirthNaturalPerson = request.Customer?.DateOfBirthNaturalPerson,
            SalesArrangementId = request.SalesArrangementId,
            CustomerRoleId = (CustomerRoles)request.CustomerRoleId,
            LockedIncomeDateTime = request.Customer?.LockedIncomeDateTime,
            MaritalStatusId = request.Customer?.MaritalStatusId,
            Identities = request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        bool containsKbIdentity = request
            .Customer?
            .CustomerIdentifiers?
            .Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb) ?? false;

        bool containsMpIdentity = request
            .Customer?
            .CustomerIdentifiers?
            .Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp) ?? false;

        // provolat sulm
        if (containsKbIdentity)
        {
            var kbIdentityId = request
                .Customer!
                .CustomerIdentifiers
                .First(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
                .IdentityId;

            await _sulmClient.StartUse(kbIdentityId, ExternalServices.Sulm.V1.ISulmClient.PurposeMPAP, cancellationToken);
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (containsKbIdentity && !containsMpIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == IdentitySchemes.Kb);
            await _updateService.GetCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellationToken);

            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity, cancellationToken);
        }
        // nove byl customer identifikovan KB identitou
        else if (containsKbIdentity)
        {
            var kbIdentityId = entity.Identities!.First(t => t.IdentityScheme == IdentitySchemes.Kb).IdentityId;
            await _updateService.GetCustomerAndUpdateEntity(entity, kbIdentityId, IdentitySchemes.Kb, cancellationToken);
        }

        // additional data - set defaults
        // https://jira.kb.cz/browse/HFICH-4551
        CustomerAdditionalData additionalData = new()
        {
            IsAddressWhispererUsed = false,
            HasRelationshipWithKB = false,
            HasRelationshipWithKBEmployee = false,
            HasRelationshipWithCorporate = false,
            IsPoliticallyExposed = false,
            IsUSPerson = false,
            LegalCapacity = new()
            {
                RestrictionTypeId = 2
            }
        };
        entity.AdditionalData = Newtonsoft.Json.JsonConvert.SerializeObject(additionalData);
        entity.AdditionalDataBin = additionalData.ToByteArray();

        // ulozit do DB
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        model.CustomerOnSAId = entity.CustomerOnSAId;

        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSA), entity.CustomerOnSAId);

        if (entity.Identities is not null)
        {
            model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
                IdentityId = t.IdentityId
            }).ToList());
        }

        return model;
    }

    private readonly SulmService.ISulmClientHelper _sulmClient;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly UpdateCustomerService _updateService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        SulmService.ISulmClientHelper sulmClient,
        UpdateCustomerService updateService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateCustomerHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
        _logger = logger;
    }
}