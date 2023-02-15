namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

public sealed class GetDetailWithChangesResponse
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailConfirmedContacts
{
    public SharedDto.EmailAddressConfirmedDto? PrimaryEmail { get; set; }

    public SharedDto.PhoneNumberConfirmedDto? PrimaryPhoneNumber { get; set; }
}
