using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Telemetry.AuditLog;
using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomer;

internal sealed class UpdateCustomerHandler
    : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // helper aby se nemuselo porad null checkovat
        entity.Identities ??= new List<Database.Entities.CustomerOnSAIdentity>();

        // customerOnSA byl jiz updatovan z KB CM
        bool alreadyKbUpdatedCustomer = entity.Identities.Any(t => t.IdentityScheme == IdentitySchemes.Kb);

        // vychazim z toho, ze identitu klienta nelze menit. Tj. z muze prijit prazdna kolekce CustomerIdentifiers v requestu, ale to neznamena, ze jiz existujici identity na COnSA odstranim.
        if (request.Customer.CustomerIdentifiers is not null && request.Customer.CustomerIdentifiers.Any())
        {
            // kontrola, zda dane schema jiz nema s jinym ID
            if (request.Customer.CustomerIdentifiers.Any(t => entity.Identities.Any(x => (int)x.IdentityScheme == (int)t.IdentityScheme && x.IdentityId != t.IdentityId)))
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.IdentityAlreadyExistOnCustomer);
            }

            var existingSchemas = entity.Identities.Select(t => (int)t.IdentityScheme).ToList();
            var newSchemasToAdd = request.Customer.CustomerIdentifiers.Where(t => !existingSchemas.Contains((int)t.IdentityScheme)).ToList();

            entity.Identities.AddRange(newSchemasToAdd.Select(t => new Database.Entities.CustomerOnSAIdentity(t, entity.CustomerOnSAId)));
        }

        // provolat sulm - pokud jiz ma nebo mu byla akorat pridana KB identita
        var kbIdentity = entity.Identities.FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);
        if (!alreadyKbUpdatedCustomer && kbIdentity is not null)
        {
            await _sulmClient.StartUse(kbIdentity.IdentityId, ExternalServices.Sulm.V1.ISulmClient.PurposeMPAP, cancellationToken);
        }

        // uz ma KB identitu, ale jeste nema MP identitu
        if (!entity.Identities.Any(t => t.IdentityScheme == IdentitySchemes.Mp) && kbIdentity is not null)
        {
            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity, cancellationToken);
        }

        // update CustomerOnSA
        entity.DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson;
        entity.FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson ?? "";
        entity.Name = request.Customer.Name ?? "";
        entity.MaritalStatusId = request.Customer.MaritalStatusId;
        entity.LockedIncomeDateTime = request.Customer.LockedIncomeDateTime;

        await _dbContext.SaveChangesAsync(cancellationToken);

        // response instance
        var model = new UpdateCustomerResponse();
        model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity
        {
            IdentityScheme = (CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
            IdentityId = t.IdentityId
        }).ToList());

        // auditni log
        if (!alreadyKbUpdatedCustomer && kbIdentity is not null)
        {
            var caseId = (await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellationToken)).CaseId;

            _auditLogger.LogWithCurrentUser(
                AuditEventTypes.Noby006,
                "Identifikovaný klient byl přiřazen k žádosti",
                identities: new List<AuditLoggerHeaderItem>
                {
                    new(kbIdentity.IdentityScheme.ToString(), kbIdentity.IdentityId)
                },
                products: new List<AuditLoggerHeaderItem>
                {
                    new("case", caseId),
                    new("salesArrangement", entity.SalesArrangementId)
                }
            );
        }

        return model;
    }

    private readonly IAuditLogger _auditLogger;
    private readonly SulmService.ISulmClientHelper _sulmClient;
    private readonly UpdateCustomerService _updateService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public UpdateCustomerHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        IAuditLogger auditLogger,
        SulmService.ISulmClientHelper sulmClient,
        UpdateCustomerService updateService,
        Database.HouseholdServiceDbContext dbContext)
    {
        _salesArrangementService = salesArrangementService;
        _auditLogger = auditLogger;
        _sulmClient = sulmClient;
        _updateService = updateService;
        _dbContext = dbContext;
    }
}