﻿using DomainServices.CaseService.Abstraction;
using DomainServices.CustomerService.Clients;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Customer = DomainServices.CustomerService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.Shared;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class UpdateCustomerService
{
    public async Task GetCustomerAndUpdateEntity(Repositories.Entities.CustomerOnSA entity, long identityId, CIS.Foms.Enums.IdentitySchemes scheme, CancellationToken cancellation)
    {
        if (_cachedCustomerInstance is not null) return;

        var kbIdentity = new CIS.Infrastructure.gRPC.CisTypes.Identity(identityId, scheme);

        _cachedCustomerInstance = ServiceCallResult.ResolveAndThrowIfError<_Customer.CustomerDetailResponse>(await _customerService.GetCustomerDetail(kbIdentity, cancellation));

        // propsat udaje do customerOnSA
        entity.DateOfBirthNaturalPerson = _cachedCustomerInstance.NaturalPerson?.DateOfBirth;
        entity.FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName;
        entity.Name = _cachedCustomerInstance.NaturalPerson?.LastName ?? "";
        entity.MaritalStatusId = _cachedCustomerInstance.NaturalPerson?.MaritalStatusStateId;

        // get CaseId
        if (entity.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor)
        {
            var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellation));

            // update case service
            await _caseService.UpdateCaseCustomer(saInstance.CaseId, new CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = _cachedCustomerInstance.NaturalPerson?.DateOfBirth,
                FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName,
                Name = _cachedCustomerInstance.NaturalPerson?.LastName,
                Identity = kbIdentity
            }, cancellation);
        }
    }

    public async Task TryCreateMpIdentity(Repositories.Entities.CustomerOnSA entity)
    {
        int? id = resolveCreateEasClient(await _easClient.CreateNewOrGetExisingClient(getEasClientModel()));

        if (id.HasValue)
        {
            entity.Identities ??= new List<Repositories.Entities.CustomerOnSAIdentity>();
            entity.Identities.Add(new Repositories.Entities.CustomerOnSAIdentity
            {
                CustomerOnSAId = entity.CustomerOnSAId,
                IdentityId = id.Value,
                IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp
            });
        }
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

    private _Customer.CustomerDetailResponse? _cachedCustomerInstance;
    
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerServiceClient _customerService;
    private readonly Eas.IEasClient _easClient;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public UpdateCustomerService(
        Eas.IEasClient easClient,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ICaseServiceAbstraction caseService,
        ICustomerServiceClient customerService,
        Repositories.HouseholdServiceDbContext dbContext)
    {
        _salesArrangementService = salesArrangementService;
        _easClient = easClient;
        _caseService = caseService;
        _customerService = customerService;
        _dbContext = dbContext;
    }
}
