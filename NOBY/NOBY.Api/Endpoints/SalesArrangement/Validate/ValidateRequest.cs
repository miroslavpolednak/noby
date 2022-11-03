namespace NOBY.Api.Endpoints.SalesArrangement.Validate;

internal record ValidateRequest(int SalesArrangementId)
    : IRequest<ValidateResponse>
{
}
