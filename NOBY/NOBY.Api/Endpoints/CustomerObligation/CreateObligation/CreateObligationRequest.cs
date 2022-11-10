using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.CustomerObligation.CreateObligation;

public sealed class CreateObligationRequest
    : Dto.ObligationDto, IRequest<int>
{
    [JsonIgnore]
    internal int? CustomerOnSAId;

    internal CreateObligationRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
