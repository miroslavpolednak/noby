using SharedAudit;
using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;
using CIS.Infrastructure.Caching.Grpc;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateCustomer;

internal sealed class UpdateCustomerHandler(
    IGrpcServerResponseCache _responseCache,
    SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    IAuditLogger _auditLogger,
    SulmService.ISulmClientHelper _sulmClient,
    UpdateCustomerService _updateService,
    Database.HouseholdServiceDbContext _dbContext)
        : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // helper aby se nemuselo porad null checkovat
        entity.Identities ??= new List<Database.Entities.CustomerOnSAIdentity>();

        // customerOnSA byl jiz updatovan z KB CM
        bool alreadyKbUpdatedCustomer = entity.Identities.Any(t => t.IdentityScheme == IdentitySchemes.Kb);

        // v tomto pripade natvrdo beru identity z requestu a nezajima me, jake mel pred tim
        if (request.SkipValidations)
        {
            entity.Identities.Clear();
            if (request.Customer.CustomerIdentifiers is not null)
            {
                foreach (var identity in request.Customer.CustomerIdentifiers)
                {
                    entity.Identities.AddRange(request.Customer.CustomerIdentifiers.Select(t => new Database.Entities.CustomerOnSAIdentity(t, entity.CustomerOnSAId)));
                }
            }
        }
        // vychazim z toho, ze identitu klienta nelze menit. Tj. z muze prijit prazdna kolekce CustomerIdentifiers v requestu, ale to neznamena, ze jiz existujici identity na COnSA odstranim.
        else if (request.Customer.CustomerIdentifiers is not null && request.Customer.CustomerIdentifiers.Count != 0)
        {
            // kontrola, zda dane schema jiz nema s jinym ID
            if (request.Customer.CustomerIdentifiers.Any(t => entity.Identities.Any(x => (int)x.IdentityScheme == (int)t.IdentityScheme && x.IdentityId != t.IdentityId)))
            {
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.IdentityAlreadyExistOnCustomer);
            }

            var existingSchemas = entity.Identities.Select(t => (int)t.IdentityScheme).ToList();
            var newSchemasToAdd = request.Customer.CustomerIdentifiers.Where(t => !existingSchemas.Contains((int)t.IdentityScheme)).ToList();

            entity.Identities.AddRange(newSchemasToAdd.Select(t => new Database.Entities.CustomerOnSAIdentity(t, entity.CustomerOnSAId)));
        }

        // provolat sulm - pokud jiz ma nebo mu byla akorat pridana KB identita
        var kbIdentity = entity.Identities.FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);
        if (!alreadyKbUpdatedCustomer && kbIdentity is not null)
        {
            await _sulmClient.StartUse(kbIdentity.IdentityId, SulmService.ISulmClient.PurposeMPAP, cancellationToken);
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
        model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new SharedTypes.GrpcTypes.Identity
        {
            IdentityScheme = (SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes)(int)t.IdentityScheme,
            IdentityId = t.IdentityId
        }).ToList());

        // auditni log
        if (!alreadyKbUpdatedCustomer && kbIdentity is not null)
        {
            var caseId = (await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellationToken)).CaseId;

            _auditLogger.Log(
                AuditEventTypes.Noby006,
                "Identifikovaný klient byl přiřazen k žádosti",
                identities: new List<AuditLoggerHeaderItem>
                {
                    new("KBID", kbIdentity.IdentityId)
                },
                products: new List<AuditLoggerHeaderItem>
                {
                    new(AuditConstants.ProductNamesCase, caseId),
                    new(AuditConstants.ProductNamesSalesArrangement, entity.SalesArrangementId)
                }
            );
        }

        await _responseCache.InvalidateEntry(nameof(GetCustomerList), entity.SalesArrangementId);

        return model;
    }
}