using DomainServices.CustomerService.Abstraction;
using _Customer = DomainServices.CustomerService.Contracts;
using Microsoft.EntityFrameworkCore;
using _SA = DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Abstraction;

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
            await getCustomerAndUpdateEntity(entity, identity.IdentityId, identity.IdentityScheme, cancellation);

            // zavolat EAS
            int? newMpIdentityId = resolveCreateEasClient(await _easClient.CreateNewOrGetExisingClient(getEasClientModel()));

            // pokud probehlo zadani klienta v eas ok, tak pridej schema
            if (newMpIdentityId.HasValue)
            {
                model.PartnerId = newMpIdentityId.Value;
                entity.Identities.Add(new Repositories.Entities.CustomerOnSAIdentity
                {
                    CustomerOnSAId = entity.CustomerOnSAId,
                    IdentityId = newMpIdentityId.Value,
                    IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp
                });
            }
        }
        // nove byl customer identifikovan KB identitou
        else if (!alreadyKbUpdatedCustomer && entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb))
        {
            await getCustomerAndUpdateEntity(entity, entity.Identities.First(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb).IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb, cancellation);
        }

        // update CustomerOnSA
        entity.HasPartner = request.Request.Customer.HasPartner;
        entity.LockedIncomeDateTime = request.Request.Customer.LockedIncomeDateTime;

        await _dbContext.SaveChangesAsync(cancellation);

        return model;
    }

    async Task getCustomerAndUpdateEntity(Repositories.Entities.CustomerOnSA entity, long identityId, CIS.Foms.Enums.IdentitySchemes scheme, CancellationToken cancellation)
    {
        if (_cachedCustomerInstance is not null) return;

        var kbIdentity = new CIS.Infrastructure.gRPC.CisTypes.Identity(identityId, scheme);

        _cachedCustomerInstance = ServiceCallResult.ResolveAndThrowIfError<_Customer.CustomerResponse>(await _customerService.GetCustomerDetail(new()
        {
            Identity = kbIdentity
        }, cancellation));

        // propsat udaje do customerOnSA
        entity.DateOfBirthNaturalPerson = _cachedCustomerInstance.NaturalPerson?.DateOfBirth;
        entity.FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName;
        entity.Name = _cachedCustomerInstance.NaturalPerson?.LastName ?? "";

        // get CaseId
        var caseId = await _dbContext.SalesArrangements.Where(t => entity.SalesArrangementId == t.SalesArrangementId).Select(t => t.CaseId).FirstAsync(cancellation);

        // update case service
        await _caseService.UpdateCaseCustomer(caseId, new CaseService.Contracts.CustomerData
        {
            DateOfBirthNaturalPerson = _cachedCustomerInstance.NaturalPerson?.DateOfBirth,
            FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName,
            Name = _cachedCustomerInstance.NaturalPerson?.LastName,
            Identity = kbIdentity
        }, cancellation);
    }

    private ExternalServices.Eas.Dto.ClientDataModel getEasClientModel()
        => new()
        {
            BirthNumber = _cachedCustomerInstance!.NaturalPerson!.BirthNumber,
            FirstName = _cachedCustomerInstance.NaturalPerson.FirstName,
            LastName = _cachedCustomerInstance.NaturalPerson.LastName,
            DateOfBirth = _cachedCustomerInstance.NaturalPerson.DateOfBirth
        };

    // zalozit noveho klienta v EAS
    static int? resolveCreateEasClient(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model.Id,
            ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
            _ => throw new NotImplementedException("resolveCreateEasClient")
        };

    private _Customer.CustomerResponse? _cachedCustomerInstance;

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly Eas.IEasClient _easClient;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public UpdateCustomerHandler(
        Eas.IEasClient easClient,
        ICaseServiceAbstraction caseService,
        ICustomerServiceAbstraction customerService,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _easClient = easClient;
        _caseService = caseService;
        _customerService = customerService;
        _dbContext = dbContext;
    }
}