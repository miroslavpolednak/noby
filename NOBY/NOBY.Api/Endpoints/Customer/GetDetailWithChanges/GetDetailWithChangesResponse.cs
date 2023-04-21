namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

public sealed class GetDetailWithChangesResponse
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailConfirmedContacts
{
    public SharedDto.EmailAddressConfirmedDto? EmailAddress { get; set; }

    public SharedDto.PhoneNumberConfirmedDto? MobilePhone { get; set; }
}
