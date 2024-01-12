using SharedAudit;
using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Api.Database.Entities;
using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal sealed class CreateCustomerHandler
    : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        // check existing SalesArrangementId
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var entity = new Database.Entities.CustomerOnSA
        {
            FirstNameNaturalPerson = request.Customer?.FirstNameNaturalPerson ?? "",
            Name = request.Customer?.Name ?? "",
            DateOfBirthNaturalPerson = request.Customer?.DateOfBirthNaturalPerson,
            SalesArrangementId = request.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            CustomerRoleId = (CustomerRoles)request.CustomerRoleId,
            LockedIncomeDateTime = request.Customer?.LockedIncomeDateTime,
            MaritalStatusId = request.Customer?.MaritalStatusId,
            Identities = request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        var kbIdentity = request
            .Customer?
            .CustomerIdentifiers?
            .FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb);

        bool containsMpIdentity = request
            .Customer?
            .CustomerIdentifiers?
            .Any(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp) ?? false;

        // kontrola zda customer existuje v CM
        if (kbIdentity is not null)
        {
            await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

            // provolat sulm
            await _sulmClient.StartUse(kbIdentity.IdentityId, ExternalServices.Sulm.V1.ISulmClient.PurposeMPAP, cancellationToken);

            // uz ma KB identitu, ale jeste nema MP identitu
            if (!containsMpIdentity)
            {
                // zavolat EAS
                await _updateService.TryCreateMpIdentity(entity, cancellationToken);
            }
        }

        // ulozit do DB
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSA), entity.CustomerOnSAId);

        // ulozit document data
        await saveDocumentData(entity.CustomerOnSAId, cancellationToken);

        // update case detailu
        if (kbIdentity is not null && entity.CustomerRoleId == CustomerRoles.Debtor)
        {
            await updateCase(salesArrangement.CaseId, entity, kbIdentity, cancellationToken);
        }

        var model = new CreateCustomerResponse
        {
            CustomerOnSAId = entity.CustomerOnSAId
        };
        if (entity.Identities is not null)
        {
            model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new SharedTypes.GrpcTypes.Identity
            {
                IdentityScheme = (SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
                IdentityId = t.IdentityId
            }).ToList());
        }

        // auditni log
        if (kbIdentity is not null)
        {
            _auditLogger.Log(
                AuditEventTypes.Noby006,
                "Identifikovaný klient byl přiřazen k žádosti",
                identities: new List<AuditLoggerHeaderItem>
                {
                    new("KBID", kbIdentity.IdentityId)
                },
                products: new List<AuditLoggerHeaderItem>
                {
                    new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
                    new(AuditConstants.ProductNamesSalesArrangement, salesArrangement.SalesArrangementId)
                }
            );
        }

        return model;
    }

    private async Task saveDocumentData(int customerOnSAId, CancellationToken cancellationToken)
    {
        Database.DocumentDataEntities.CustomerOnSAData documentEntity = new()
        {
            AdditionalData = new()
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
            }
        };

        await _documentDataStorage.Add(customerOnSAId, documentEntity, cancellationToken);
    }

    private async Task updateCase(long caseId, Database.Entities.CustomerOnSA entity, SharedTypes.GrpcTypes.Identity identity, CancellationToken cancellationToken)
    {
        // update case service
        await _caseService.UpdateCustomerData(caseId, new CaseService.Contracts.CustomerData
        {
            DateOfBirthNaturalPerson = entity.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = entity.FirstNameNaturalPerson,
            Name = entity.Name,
            Identity = identity
        }, cancellationToken);
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IAuditLogger _auditLogger;
    private readonly ICaseServiceClient _caseService;
    private readonly SulmService.ISulmClientHelper _sulmClient;
    private readonly ICustomerServiceClient _customerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly UpdateCustomerService _updateService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(
        IDocumentDataStorage documentDataStorage,
        IAuditLogger auditLogger,
        ICustomerServiceClient customerService,
        ISalesArrangementServiceClient salesArrangementService,
        SulmService.ISulmClientHelper sulmClient,
        UpdateCustomerService updateService,
        ICaseServiceClient caseService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateCustomerHandler> logger)
    {
        _documentDataStorage = documentDataStorage;
        _auditLogger = auditLogger;
        _caseService = caseService;
        _customerService = customerService;
        _salesArrangementService = salesArrangementService;
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
        _logger = logger;
    }
}