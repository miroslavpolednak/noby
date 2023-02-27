using CIS.Foms.Enums;
using FastEnumUtility;
using __MpHome = ExternalServices.MpHome.V1_1.Contracts;

namespace DomainServices.CustomerService.Api;

public static class CMExtensions
{
    public static string? ToCMString(this string? str)
    {
        return string.IsNullOrWhiteSpace(str) ? null : str;
    }

    public static DomainServices.CustomerService.Contracts.Contact ToContract(this Services.KonsDb.Dto.PartnerContact contact)
    {
        var item = new Contact
        {
            IsPrimary = contact.IsPrimaryContact,
            ContactTypeId = contact.ContactType
        };

        switch (item.ContactTypeId)
        {
            case (int)ContactTypes.Mobil:
                if (string.IsNullOrEmpty(contact.Value))
                    item.Mobile = new MobilePhoneItem();
                else if (contact.Value.Length == 9)
                    item.Mobile = new MobilePhoneItem { PhoneNumber = contact.Value, PhoneIDC = "" };
                else
                    item.Mobile = new MobilePhoneItem { PhoneNumber = contact.Value[^9..], PhoneIDC = contact.Value[..^9] };
                break;

            case (int)ContactTypes.Email:
                item.Email = new EmailAddressItem { EmailAddress = contact.Value ?? "" };
                break;

            default:
                throw new NotImplementedException($"ContactTypeId {item.ContactTypeId} not implemented");
        }

        return item;
    }

    public static __MpHome.ContactRequest ToExternalService(this DomainServices.CustomerService.Contracts.Contact contact, List<CodebookService.Contracts.Endpoints.ContactTypes.ContactTypeItem> contactTypes)
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