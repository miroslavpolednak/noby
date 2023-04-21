using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.UpdateDetailWithChanges;

public sealed class UpdateDetailWithChangesRequest
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailContacts, IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public SharedDto.EmailAddressDto? EmailAddress { get; set; }

    public SharedDto.PhoneNumberDto? MobilePhone { get; set; }

    internal UpdateDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
