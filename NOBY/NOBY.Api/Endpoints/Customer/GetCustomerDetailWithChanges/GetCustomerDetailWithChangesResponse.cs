namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

public sealed class GetCustomerDetailWithChangesResponse
    : SharedDto.BaseCustomerDetail, SharedDto.ICustomerDetailConfirmedContacts
{
    public NOBY.Dto.EmailAddressConfirmedDto? EmailAddress { get; set; }

    public NOBY.Dto.PhoneNumberConfirmedDto? MobilePhone { get; set; }
}
