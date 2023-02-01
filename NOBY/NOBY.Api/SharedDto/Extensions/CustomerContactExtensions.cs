using CIS.Foms.Enums;
using Google.Protobuf.Collections;

namespace NOBY.Api.SharedDto;

internal static class CustomerContactExtensions
{
    public static ConfirmedContactsDto? ToResponseDto(this RepeatedField<DomainServices.CustomerService.Contracts.Contact>? contacts)
    {
        if (contacts is null || contacts.Count == 0)
            return null;

        var model = new SharedDto.ConfirmedContactsDto();

        var email = contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);
        if (!string.IsNullOrEmpty(email?.Email?.Address))
        {
            model.EmailAddress = new() 
            { 
                EmailAddress = email.Email.Address,
                IsConfirmed = email.IsConfirmed
            };
        }

        var phone = contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
        if (!string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber))
        {
            model.PhoneNumber = new()
            {
                PhoneNumber = phone.Mobile.PhoneNumber,
                PhoneIDC = phone.Mobile.PhoneNumber,
                IsConfirmed = phone.IsConfirmed
            };
        }

        return model;
    }
}
