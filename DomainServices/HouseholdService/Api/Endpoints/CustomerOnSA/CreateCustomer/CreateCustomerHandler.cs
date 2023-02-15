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
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson ?? "",
            Name = request.Customer.Name ?? "",
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,
            SalesArrangementId = request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.CustomerRoleId,
            LockedIncomeDateTime = request.Customer?.LockedIncomeDateTime,
            MaritalStatusId = request.Customer?.MaritalStatusId,
            Identities = request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        bool containsKbIdentity = request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb) ?? false;
        bool containsMpIdentity = request.Customer?.CustomerIdentifiers?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp) ?? false;

        // provolat sulm
        if (containsKbIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _sulmClient.StopUse(identity.IdentityId, "MPAP", cancellationToken);
            await _sulmClient.StartUse(identity.IdentityId, "MPAP", cancellationToken);
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (containsKbIdentity && !containsMpIdentity)
        {
            var identity = entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
            await _updateService.GetCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellationToken);

            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity, cancellationToken);
        }
        // nove byl customer identifikovan KB identitou
        else if (containsKbIdentity)
        {
            await _updateService.GetCustomerAndUpdateEntity(entity, entity.Identities!.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb).IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb, cancellationToken);
        }

        // additional data
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
            model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
                IdentityId = t.IdentityId
            }).ToList());

        return model;
    }

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly UpdateCustomerService _updateService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        SulmService.ISulmClient sulmClient,
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