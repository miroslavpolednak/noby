using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients.v1;
using ExternalServices.Eas.V1;

namespace DomainServices.HouseholdService.Api.Services;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class UpdateCustomerService(
    IEasClient _easClient,
    ICustomerServiceClient _customerService,
    ICodebookServiceClient _codebookService)
{
    public async Task TryCreateMpIdentity(Database.Entities.CustomerOnSA entity, CancellationToken cancellationToken)
    {
        var dbIdentity = entity.Identities?.FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);
        if (dbIdentity is null)
            return;

        int defaultCountry = (await _codebookService.Countries(cancellationToken)).First(t => t.IsDefault).Id;
        var cmCustomer = await _customerService.GetCustomerDetail(new SharedTypes.GrpcTypes.Identity(dbIdentity.IdentityId, dbIdentity.IdentityScheme), cancellationToken);

        var model = new ExternalServices.Eas.Dto.ClientDataModel()
        {
            KbId = dbIdentity.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            BirthNumber = cmCustomer!.NaturalPerson!.BirthNumber,
            FirstName = cmCustomer.NaturalPerson.FirstName,
            LastName = cmCustomer.NaturalPerson.LastName,
            DateOfBirth = cmCustomer.NaturalPerson.DateOfBirth,
            Gender = (Genders)cmCustomer.NaturalPerson.GenderId,
            // firmu neresime?
            ClientType = string.IsNullOrWhiteSpace(cmCustomer.NaturalPerson.BirthNumber)
                ? ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.Foreigner
                : ExternalServices.Eas.Dto.ClientDataModel.ClientTypes.FO
        };

        var createdClient = await _easClient.CreateNewOrGetExisingClient(model, cancellationToken);
        
        // kontrola zda si EAS nepriradilo jine KBID
        if (createdClient.KbId.GetValueOrDefault() > 0 && dbIdentity.IdentityId != createdClient.KbId)
        {
            throw new CisValidationException(ErrorCodeMapper.EasKbDifference, $"Vybraného klienta nelze použít, použijte místo něj klienta {createdClient.FirstName} {createdClient.LastName}, {createdClient.BirthNumber} s KBID {createdClient.KbId}.");
        }

        if ((createdClient?.Id ?? 0) > 0)
        {
            entity.Identities ??= [];
            entity.Identities.Add(new Database.Entities.CustomerOnSAIdentity
            {
                CustomerOnSAId = entity.CustomerOnSAId,
                IdentityId = createdClient!.Id,
                IdentityScheme = IdentitySchemes.Mp,
            });
        }
    }
}