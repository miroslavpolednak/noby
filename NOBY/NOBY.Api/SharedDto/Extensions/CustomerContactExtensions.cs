using CIS.Foms.Enums;
using Google.Protobuf.Collections;

namespace NOBY.Api.SharedDto;

internal static class CustomerContactExtensions
{
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
                IsConfirmed = email.IsConfirmed
            };
        }

        var phone = contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
        if (!string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber))
        {
            model.MobilePhone = new()
            {
                PhoneNumber = phone.Mobile.PhoneNumber,
                PhoneIDC = phone.Mobile.PhoneIDC,
                IsConfirmed = phone.IsConfirmed
            };
        }

        return model;
    }
}
