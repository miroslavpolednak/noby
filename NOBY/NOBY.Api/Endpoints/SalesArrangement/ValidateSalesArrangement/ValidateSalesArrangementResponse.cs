using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;

public sealed class ValidateSalesArrangementResponse
{
    public List<ValidateCategory>? Categories { get; set; }
}
