using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.UpdateDetailWithChanges;

public sealed class UpdateDetailWithChangesRequest
    : IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    internal UpdateDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
