namespace NOBY.Api.Endpoints.Cases.GetCurrentPriceException;

internal sealed record GetCurrentPriceExceptionRequest(long CaseId, int SalesArrangementId)
    : IRequest<GetCurrentPriceExceptionResponse>
{
}
