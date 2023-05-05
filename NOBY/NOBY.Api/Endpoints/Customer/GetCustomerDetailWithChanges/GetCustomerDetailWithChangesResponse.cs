namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

public sealed class GetCustomerDetailWithChangesResponse
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailConfirmedContacts
{
    public SharedDto.EmailAddressConfirmedDto? EmailAddress { get; set; }

    public SharedDto.PhoneNumberConfirmedDto? MobilePhone { get; set; }
}
