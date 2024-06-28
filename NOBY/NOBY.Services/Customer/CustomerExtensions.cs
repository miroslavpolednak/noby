using Google.Protobuf.Collections;
using NOBY.Dto;

namespace NOBY.Services.Customer;

public static class CustomerExtensions
{
    public static void FillResponseDto(this DomainServices.CustomerService.Contracts.NaturalPerson person, NOBY.Dto.BaseNaturalPerson transformedModel)
    {
        transformedModel.FirstName = person.FirstName;
        transformedModel.LastName = person.LastName;
        transformedModel.DateOfBirth = person.DateOfBirth;
        transformedModel.DegreeAfterId = person.DegreeAfterId;
        transformedModel.DegreeBeforeId = person.DegreeBeforeId;
        transformedModel.MaritalStatusId = person.MaritalStatusStateId;
        transformedModel.BirthName = person.BirthName;
        transformedModel.BirthNumber = person.BirthNumber;
        transformedModel.PlaceOfBirth = person.PlaceOfBirth;
        transformedModel.Gender = (SharedTypes.Enums.Genders)person.GenderId;
        transformedModel.BirthCountryId = person.BirthCountryId;
        transformedModel.CitizenshipCountriesId = person.CitizenshipCountriesId?.Select(t => t).ToList();
    }

    public static ContactsConfirmedDto? ToResponseDto(this RepeatedField<DomainServices.CustomerService.Contracts.Contact>? contacts)
    {
        if (contacts is null || contacts.Count == 0)
            return null;

        var model = new ContactsConfirmedDto();

        var email = contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);
        if (!string.IsNullOrEmpty(email?.Email?.EmailAddress))
        {
            model.EmailAddress = new()
            {
                EmailAddress = email.Email.EmailAddress,
                IsConfirmed = email.Email.IsEmailConfirmed
            };
        }

        var phone = contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
        if (!string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber))
        {
            model.MobilePhone = new()
            {
                PhoneNumber = phone.Mobile.PhoneNumber,
                PhoneIDC = phone.Mobile.PhoneIDC,
                IsConfirmed = phone.Mobile.IsPhoneConfirmed
            };
        }

        return model;
    }

    public static DomainServices.CustomerService.Contracts.IdentificationDocument ToDomainService(this IdentificationDocumentFull document)
        => new()
        {
            IssuingCountryId = document.IssuingCountryId,
            IdentificationDocumentTypeId = document.IdentificationDocumentTypeId.GetValueOrDefault(),
            IssuedBy = document.IssuedBy,
            Number = document.Number,
            IssuedOn = document.IssuedOn,
            ValidTo = document.ValidTo
        };

    public static IdentificationDocumentFull ToResponseDto(this DomainServices.CustomerService.Contracts.IdentificationDocument document)
        => new()
        {
            IssuingCountryId = document.IssuingCountryId ?? 0,
            IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
            IssuedBy = document.IssuedBy,
            IssuedOn = document.IssuedOn,
            RegisterPlace = document.RegisterPlace,
            ValidTo = document.ValidTo,
            Number = document.Number
        };

    public static ApiContracts.SharedTypesIdentificationDocumentFull ToFeApi(this DomainServices.CustomerService.Contracts.IdentificationDocument document)
        => new()
        {
            IssuingCountryId = document.IssuingCountryId ?? 0,
            IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
            IssuedBy = document.IssuedBy,
            IssuedOn = document.IssuedOn,
            RegisterPlace = document.RegisterPlace,
            ValidTo = document.ValidTo,
            Number = document.Number
        };
}