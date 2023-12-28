using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.CustomerObligation.CreateObligation;

public sealed class CreateObligationRequest
    : SharedDto.ObligationDto, IRequest<int>
{
    [JsonIgnore]
    internal int? CustomerOnSAId;

    internal CreateObligationRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
