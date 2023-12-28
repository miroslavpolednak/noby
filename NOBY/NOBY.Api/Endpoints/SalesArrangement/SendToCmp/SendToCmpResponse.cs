using NOBY.Api.Endpoints.SalesArrangement.SharedDto;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed class SendToCmpResponse
{
    public List<ValidateCategory>? Categories { get; set; }
}
