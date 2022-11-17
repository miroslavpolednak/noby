using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.Validate;

public sealed class ValidateResponse
{
    public List<ValidateCategory>? Categories { get; set; }
}
