using SharedAudit;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Api.Database.Entities;
using DomainServices.HouseholdService.Api.Services;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using SharedComponents.DocumentDataStorage;
using SharedTypes.Extensions;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.CreateCustomer;

internal sealed class CreateCustomerHandler(
    IDocumentDataStorage _documentDataStorage,
    IAuditLogger _auditLogger,
    ICustomerServiceClient _customerService,
    ISalesArrangementServiceClient _salesArrangementService,
    SulmService.ISulmClientHelper _sulmClient,
    UpdateCustomerService _updateService,
    ICaseServiceClient _caseService,
    Database.HouseholdServiceDbContext _dbContext,
    ILogger<CreateCustomerHandler> _logger)
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
            CustomerRoleId = (EnumCustomerRoles)request.CustomerRoleId,
            LockedIncomeDateTime = request.Customer?.LockedIncomeDateTime,
            MaritalStatusId = request.Customer?.MaritalStatusId,
            Identities = request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        var kbIdentity = request.Customer?.CustomerIdentifiers?.GetKbIdentityOrDefault();
        var containsMpIdentity = request.Customer?.CustomerIdentifiers?.HasMpIdentity() ?? false;

        // uz ma KB identitu, ale jeste nema MP identitu
        if (kbIdentity is not null && !containsMpIdentity)
        {
            // zavolat EAS
            await _updateService.TryCreateMpIdentity(entity, cancellationToken);
        }

        // kontrola zda customer existuje v CM
        if (kbIdentity is not null)
        {
            await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

            // provolat sulm
            await _sulmClient.StartUse(kbIdentity.IdentityId, SulmService.ISulmClient.PurposeMPAP, cancellationToken);
        }

        // ulozit do DB
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSA), entity.CustomerOnSAId);

        // ulozit document data
        await saveDocumentData(entity.CustomerOnSAId, cancellationToken);

        // update case detailu
        if (kbIdentity is not null && entity.CustomerRoleId == EnumCustomerRoles.Debtor)
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
                identities:
                [
                    new("KBID", kbIdentity.IdentityId)
                ],
                products:
                [
                    new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
                    new(AuditConstants.ProductNamesSalesArrangement, salesArrangement.SalesArrangementId)
                ]
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
}