using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.Customer.Identify;

public class IdentifyRequest
    : IRequest
{
    public CIS.Foms.Types.CustomerIdentity? CustomerIdentity { get; set; }

    [JsonIgnore]
    internal int CustomerOnSAId;

    internal IdentifyRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
