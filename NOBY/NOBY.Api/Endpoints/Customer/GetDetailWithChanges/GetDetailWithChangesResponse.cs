namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

public sealed class GetDetailWithChangesResponse
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailConfirmedContacts
{
    public Shared.CustomerDetailEmailConfirmedDto? PrimaryEmail { get; set; }

    public Shared.CustomerDetailPhoneConfirmedDto? PrimaryPhoneNumber { get; set; }
}
