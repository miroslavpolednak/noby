﻿using FastEnumUtility;
using __MpHome = ExternalServices.MpHome.V1.Contracts;

namespace DomainServices.CustomerService.Api;

public static class CMExtensions
{
    public static string? ToCMString(this string? str)
    {
        return string.IsNullOrWhiteSpace(str) ? null : str;
    }

    public static Contact ToContract(this __MpHome.ContactResponse contact)
    {
        var item = new Contact
        {
            IsPrimary = contact.Primary,
            ContactTypeId = (int)contact.Type
        };

        switch (item.ContactTypeId)
        {
            case (int)ContactTypes.Mobil:
                if (string.IsNullOrEmpty(contact.Value))
                    item.Mobile = new MobilePhoneItem();
                else if (contact.Value.Length == 9)
                    item.Mobile = new MobilePhoneItem { PhoneNumber = contact.Value, PhoneIDC = "" };
                else
                    item.Mobile = new MobilePhoneItem { PhoneNumber = contact.Value[^9..].Trim(), PhoneIDC = contact.Value[..^9].Trim() };
                break;

            case (int)ContactTypes.Email:
                item.Email = new EmailAddressItem { EmailAddress = contact.Value ?? "" };
                break;

            default:
                throw new NotImplementedException($"ContactTypeId {item.ContactTypeId} not implemented");
        }

        return item;
    }

    public static __MpHome.ContactRequest ToExternalService(this Contact contact, List<CodebookService.Contracts.v1.ContactTypesResponse.Types.ContactTypeItem> contactTypes)
    {
        var item = new __MpHome.ContactRequest
        {
            Type = FastEnum.Parse<__MpHome.ContactType>(contactTypes.First(x => x.Id == contact.ContactTypeId).MpDigiApiCode),
            Primary = true,
            Value = contact.DataCase switch
            {
                Contact.DataOneofCase.Email => contact.Email?.EmailAddress,
                Contact.DataOneofCase.Mobile => $"{contact.Mobile?.PhoneIDC}{contact.Mobile?.PhoneNumber}",
                _ => throw new NotImplementedException()
            }
        };

        return item;
    }
}