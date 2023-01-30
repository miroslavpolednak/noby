using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _Customer = DomainServices.CustomerService.Contracts;

namespace DomainServices.HouseholdService.Api.Services;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class UpdateCustomerService
{
    public async Task GetCustomerAndUpdateEntity(Database.Entities.CustomerOnSA entity, long identityId, CIS.Foms.Enums.IdentitySchemes scheme, CancellationToken cancellation)
    {
        await ensureLoadedCustomer(new CIS.Infrastructure.gRPC.CisTypes.Identity(identityId, scheme), cancellation);

        // propsat udaje do customerOnSA
        entity.DateOfBirthNaturalPerson = _cachedCustomerInstance!.NaturalPerson?.DateOfBirth;
        entity.FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName;
        entity.Name = _cachedCustomerInstance.NaturalPerson?.LastName ?? "";
        entity.MaritalStatusId = _cachedCustomerInstance.NaturalPerson?.MaritalStatusStateId;

        // get CaseId
        if (entity.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor)
        {
            var saInstance = await _salesArrangementService.GetSalesArrangement(entity.SalesArrangementId, cancellation);

            // update case service
            await _caseService.UpdateCustomerData(saInstance.CaseId, new CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = _cachedCustomerInstance.NaturalPerson?.DateOfBirth,
                FirstNameNaturalPerson = _cachedCustomerInstance.NaturalPerson?.FirstName,
                Name = _cachedCustomerInstance.NaturalPerson?.LastName,
                Identity = new CIS.Infrastructure.gRPC.CisTypes.Identity(identityId, scheme)
            }, cancellation);
        }
    }

    public async Task TryCreateMpIdentity(Database.Entities.CustomerOnSA entity, CancellationToken cancellationToken)
    {
        var dbIdentity = entity.Identities?.FirstOrDefault(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Kb);
        if (dbIdentity is null)
            return;

        int defaultCountry = (await _codebookService.Countries(cancellationToken)).First(t => t.IsDefault).Id;
        await ensureLoadedCustomer(new(dbIdentity.IdentityId, dbIdentity.IdentityScheme), cancellationToken);

        var model = new ExternalServices.Eas.Dto.ClientDataModel()
        {
            BirthNumber = _cachedCustomerInstance!.NaturalPerson!.BirthNumber,
            FirstName = _cachedCustomerInstance.NaturalPerson.FirstName,
            LastName = _cachedCustomerInstance.NaturalPerson.LastName,
            DateOfBirth = _cachedCustomerInstance.NaturalPerson.DateOfBirth,
            // firmu neresime?
            ClientType = _cachedCustomerInstance.NaturalPerson.CitizenshipCountriesId?.Any(t => t == defaultCountry) ?? false ? ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.FO : ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.Foreigner
        };

        int? id = (await _easClient.CreateNewOrGetExisingClient(model)).Id;

        if (id.HasValue)
        {
            entity.Identities ??= new List<Database.Entities.CustomerOnSAIdentity>();
            entity.Identities.Add(new Database.Entities.CustomerOnSAIdentity
            {
                CustomerOnSAId = entity.CustomerOnSAId,
                IdentityId = id.Value,
                IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp,
            });
        }
    }

    private async Task ensureLoadedCustomer(CIS.Infrastructure.gRPC.CisTypes.Identity identity, CancellationToken cancellationToken)
    {
        if (_cachedCustomerInstance is not null) return;
        _cachedCustomerInstance = await _customerService.GetCustomerDetail(identity, cancellationToken);
    }

    private _Customer.CustomerDetailResponse? _cachedCustomerInstance;

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerServiceClient _customerService;
    private readonly Eas.IEasClient _easClient;

    public UpdateCustomerService(
        Eas.IEasClient easClient,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService,
        ICustomerServiceClient customerService,
        DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _easClient = easClient;
        _caseService = caseService;
        _customerService = customerService;
    }
}
