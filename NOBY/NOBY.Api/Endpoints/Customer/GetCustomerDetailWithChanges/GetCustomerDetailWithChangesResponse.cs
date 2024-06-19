using NOBY.Dto.Customer;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

public sealed class GetCustomerDetailWithChangesResponse
    : BaseCustomerDetail, ICustomerDetailConfirmedContacts
{
    public NOBY.Dto.EmailAddressConfirmedDto? EmailAddress { get; set; }

    public NOBY.Dto.PhoneNumberConfirmedDto? MobilePhone { get; set; }
}
