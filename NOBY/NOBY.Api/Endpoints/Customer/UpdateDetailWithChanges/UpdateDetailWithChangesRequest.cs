using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.UpdateDetailWithChanges;

public sealed class UpdateDetailWithChangesRequest
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailContacts, IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public Shared.CustomerDetailEmailDto? PrimaryEmail { get; set; }

    public Shared.CustomerDetailPhoneDto? PrimaryPhoneNumber { get; set; }

    internal UpdateDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
