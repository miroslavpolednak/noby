using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using ExternalServices.Eas.V1;

namespace DomainServices.HouseholdService.Api.Services;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class UpdateCustomerService
{
    public async Task TryCreateMpIdentity(Database.Entities.CustomerOnSA entity, CancellationToken cancellationToken)
    {
        var dbIdentity = entity.Identities?.FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);
        if (dbIdentity is null)
            return;

        int defaultCountry = (await _codebookService.Countries(cancellationToken)).First(t => t.IsDefault).Id;
        var cmCustomer = await _customerService.GetCustomerDetail(new CIS.Infrastructure.gRPC.CisTypes.Identity(dbIdentity.IdentityId, dbIdentity.IdentityScheme), cancellationToken);

        var model = new ExternalServices.Eas.Dto.ClientDataModel()
        {
            KbId = dbIdentity.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            BirthNumber = cmCustomer!.NaturalPerson!.BirthNumber,
            FirstName = cmCustomer.NaturalPerson.FirstName,
            LastName = cmCustomer.NaturalPerson.LastName,
            DateOfBirth = cmCustomer.NaturalPerson.DateOfBirth,
            // firmu neresime?
            ClientType = cmCustomer.NaturalPerson.CitizenshipCountriesId?.Any(t => t == defaultCountry) ?? false ? ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.FO : ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.Foreigner
        };

        int? id = (await _easClient.CreateNewOrGetExisingClient(model, cancellationToken)).Id;

        if (id.HasValue)
        {
            entity.Identities ??= new List<Database.Entities.CustomerOnSAIdentity>();
            entity.Identities.Add(new Database.Entities.CustomerOnSAIdentity
            {
                CustomerOnSAId = entity.CustomerOnSAId,
                IdentityId = id.Value,
                IdentityScheme = IdentitySchemes.Mp,
            });
        }
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ICustomerServiceClient _customerService;
    private readonly IEasClient _easClient;

    public UpdateCustomerService(
        IEasClient easClient,
        ICustomerServiceClient customerService,
        ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _easClient = easClient;
        _customerService = customerService;
    }
}