using DomainServices.CustomerService.Api.ExternalServices.MpHome.MpHomeWrapper;

namespace DomainServices.CustomerService.Api.ExternalServices.MpHome;

internal static class Extensions
{
    public static PartnerRequest ToMpHomePartner(this Contracts.CreateRequest model)
    {
        // vytvorit partnerRequest
        var partner = new PartnerRequest
        {
            BirthNumber = model.BirthNumber,
            DateOfBirth = model.DateOfBirth,
            DegreeAfter = model.DegreeAfter,
            DegreeBefore = model.DegreeBefore,
            Gender = model.Gender switch
            {
                Contracts.Genders.Male => GenderEnum.Male,
                Contracts.Genders.Female => GenderEnum.Female,
                _ => GenderEnum.Unknown
            },
            Ico = model.Ico,
            Lastname = model.LastName,
            Name = model.FirstName,
            NameJuridicalPerson = model.NameJuridicalPerson,
            PlaceOfBirth = model.PlaceOfBirth,
            IdentificationDocuments = new List<IdentificationDocument>(),
            Addresses = new List<AddressData>(),
            Contacts = new List<ContactRequest>()
        };

        // pridat doklad
        if (model.IdentificationDocument != null)
            partner.IdentificationDocuments.Add(model.IdentificationDocument.ToMpHomeIdentificationDocument());

        // pridat adresy
        if (model.Addresses != null && model.Addresses.Any())
            model.Addresses.ToList().ForEach(address => partner.Addresses.Add(address.ToMpHomeAddress()));

        // pridat kontakty
        if (model.Contacts != null && model.Contacts.Any())
            model.Contacts.ToList().ForEach(contact => partner.Contacts.Add(new ContactRequest
            {
                Primary = contact.IsPrimary,
                Value = contact.Value,
                RequestedAction = ActionType.Create,
                Type = contact.Type switch
                {
                    Contracts.Contact.Types.ContactTypes.Email => ContactType.Email,
                    Contracts.Contact.Types.ContactTypes.MobilePrivate => ContactType.Mobile,
                    Contracts.Contact.Types.ContactTypes.MobileWork => ContactType.BusinessMobile,
                    Contracts.Contact.Types.ContactTypes.LandlineHome => ContactType.FixedHomeLine,
                    _ => ContactType.Unknown
                }
            }));

        return partner;
    }

    public static PartnerBaseRequest ToMpHomePartnerBase(this Contracts.CustomerInput model)
        => new PartnerBaseRequest
        {
            BirthNumber = model.BirthNumber,
            DateOfBirth = model.DateOfBirth,
            DegreeAfter = model.DegreeAfter,
            DegreeBefore = model.DegreeBefore,
            Gender = model.Gender switch
            {
                Contracts.Genders.Male => GenderEnum.Male,
                Contracts.Genders.Female => GenderEnum.Female,
                _ => GenderEnum.Unknown
            },
            Ico = model.Ico,
            Lastname = model.LastName,
            Name = model.FirstName,
            NameJuridicalPerson = model.NameJuridicalPerson,
            PlaceOfBirth = model.PlaceOfBirth
        };

    public static IdentificationDocument ToMpHomeIdentificationDocument(this Contracts.IdentificationDocument model)
        => new IdentificationDocument
        {
            Type = model.Type switch
            {
                    //TODO IdentificationDocumentTypes na MpHome IdentificationCardType
                    Contracts.IdentificationDocumentTypes.A => IdentificationCardType.IDCard,
                Contracts.IdentificationDocumentTypes.B => IdentificationCardType.Passport,
                _ => IdentificationCardType.Undefined
            },
            IssuedBy = model.IssuedBy,
            IssuedOn = model.IssuedOn,
            IssuingCountry = model.IssuingCountryCode,
            ValidTo = model.ValidTo,
            Number = model.Number
        };

    public static AddressData ToMpHomeAddress(this Contracts.Address model)
        => new AddressData
        {
            BuildingIdentificationNumber = model.BuildingIdentificationNumber,
            City = model.City,
            LandRegistryNumber = model.LandRegistryNumber,
            PostCode = model.Postcode,
            Street = model.Street,
            Type = model.Type switch
            {
                Contracts.AddressTypes.Pernament => AddressType.Permanent,
                Contracts.AddressTypes.Mailing => AddressType.Mailing,
                _ => AddressType.Unknown,
            }
        };

    public static ContactData ToMpHomeContactData(this Contracts.Contact model)
        => new ContactData
        {
            Primary = model.IsPrimary,
            Value = model.Value,
            Type = model.Type switch
            {
                Contracts.Contact.Types.ContactTypes.Email => ContactType.Email,
                Contracts.Contact.Types.ContactTypes.MobilePrivate => ContactType.Mobile,
                Contracts.Contact.Types.ContactTypes.MobileWork => ContactType.BusinessMobile,
                Contracts.Contact.Types.ContactTypes.LandlineHome => ContactType.FixedHomeLine,
                _ => ContactType.Unknown
            }
        };
}