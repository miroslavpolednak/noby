using Google.Protobuf.Collections;

namespace NOBY.Api.SharedDto;

internal static class CustomerContactExtensions
{
    public static CustomerContact ToResponseDto(this DomainServices.CustomerService.Contracts.Contact contact)
        => new()
        {
            ContactTypeId = contact.ContactTypeId,
            Value = contact.Value,
            IsPrimary = contact.IsPrimary
        };

    public static List<CustomerContact>? ToResponseDto(this RepeatedField<DomainServices.CustomerService.Contracts.Contact>? contacts)
        => contacts?.Select(t => t.ToResponseDto()).ToList();
}
