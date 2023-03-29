using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

public sealed class IdentifyByIdentityRequest
    : IRequest
{
    /// <summary>
    /// Identita klienta
    /// </summary>
    [Required]
    public CIS.Foms.Types.CustomerIdentity? CustomerIdentity { get; set; }

    [JsonIgnore]
    internal int CustomerOnSAId;

    internal IdentifyByIdentityRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
