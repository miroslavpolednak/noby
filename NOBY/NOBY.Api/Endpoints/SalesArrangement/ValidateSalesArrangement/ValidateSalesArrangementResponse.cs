using NOBY.Api.Endpoints.SalesArrangement.SharedDto;

namespace NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;

public sealed class ValidateSalesArrangementResponse
{
    public List<ValidateCategory>? Categories { get; set; }
}
