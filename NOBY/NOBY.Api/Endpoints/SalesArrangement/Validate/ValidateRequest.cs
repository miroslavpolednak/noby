namespace NOBY.Api.Endpoints.SalesArrangement.Validate;

internal sealed record ValidateRequest(int SalesArrangementId)
    : IRequest<ValidateResponse>
{
}
