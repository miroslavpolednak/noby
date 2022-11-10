using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

public class IdentifyByIdentityRequest
    : IRequest
{
    public CIS.Foms.Types.CustomerIdentity? CustomerIdentity { get; set; }

    [JsonIgnore]
    internal int CustomerOnSAId;

    internal IdentifyByIdentityRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
