using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

public sealed class UpdateCustomerDetailWithChangesRequest
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailContacts, IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public NOBY.Dto.EmailAddressDto? EmailAddress { get; set; }

    public NOBY.Dto.PhoneNumberDto? MobilePhone { get; set; }

    internal UpdateCustomerDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
