using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1.Contracts;
using FastEnumUtility;

namespace DomainServices.CustomerService.Api.Services;

[ScopedService, SelfService]
internal sealed class MpHomeUpdateMapper(ICodebookServiceClient _codebookService)
{
    public async Task<PartnerRequest> MapUpdateRequest(
        NaturalPerson naturalPerson,
        IList<SharedTypes.GrpcTypes.Identity> identities,
        Contracts.IdentificationDocument? identificationDocument,
        IList<SharedTypes.GrpcTypes.GrpcAddress>? addresses,
        IList<Contact> contacts,
        CancellationToken cancellationToken)
    {
        var titles = await _codebookService.AcademicDegreesBefore(cancellationToken);
        
        return new PartnerRequest
        {
            Name = naturalPerson.FirstName,
            Lastname = naturalPerson.LastName,
            DegreeBefore = titles.FirstOrDefault(t => t.Id == naturalPerson.DegreeBeforeId)?.Name,
            BirthNumber = naturalPerson.BirthNumber,
            DateOfBirth = naturalPerson.DateOfBirth,
            PlaceOfBirth = naturalPerson.PlaceOfBirth,
            Gender = (GenderEnum)naturalPerson.GenderId,
            NationalityId = naturalPerson.CitizenshipCountriesId.FirstOrDefault(),
            Addresses = mapAddresses(addresses),
            Contacts = await mapContacts(contacts, cancellationToken),
            KbId = identities.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId,
            IdentificationDocuments = await mapIdentificationDocument(identificationDocument, cancellationToken)
        };
    }

    private async Task<List<ContactRequest>?> mapContacts(
        IList<Contact> contacts,
        CancellationToken cancellationToken)
    {
        var contactTypes = await _codebookService.ContactTypes(cancellationToken);

        return contacts?.Select(contact => contact.ToExternalService(contactTypes)).ToList();
    }

    private async Task<List<global::ExternalServices.MpHome.V1.Contracts.IdentificationDocument>?> mapIdentificationDocument(
        Contracts.IdentificationDocument? identificationDocument,
        CancellationToken cancellationToken)
    {
        if (identificationDocument is null)
            return null;

        var docTypes = await _codebookService.IdentificationDocumentTypes(cancellationToken);

        return
        [
            new()
            {
                Number = identificationDocument.Number,
                Type = FastEnum.Parse<IdentificationCardType>(docTypes.First(d => d.Id == identificationDocument.IdentificationDocumentTypeId).MpDigiApiCode),
                ValidTo = identificationDocument.ValidTo,
                IssuedBy = identificationDocument.IssuedBy,
                IssuedOn = identificationDocument.IssuedOn,
                IssuingCountryId = identificationDocument.IssuingCountryId ?? 0
            }
        ];
    }

    private static List<AddressData>? mapAddresses(IList<SharedTypes.GrpcTypes.GrpcAddress>? addresses)
    {
        return addresses?.Select(address => new AddressData
            {
                Type = (AddressType)(address.AddressTypeId ?? (int)AddressType.Permanent),
                Street = address.Street,
                BuildingIdentificationNumber = address.StreetNumber,
                LandRegistryNumber = string.IsNullOrWhiteSpace(address.EvidenceNumber) ? address.HouseNumber : address.EvidenceNumber,
                PostCode = address.Postcode,
                City = address.City,
                CountryId = address.CountryId
            })
            .ToList();
    }
}