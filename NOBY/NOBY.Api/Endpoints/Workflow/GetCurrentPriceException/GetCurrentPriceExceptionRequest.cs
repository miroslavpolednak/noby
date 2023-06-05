namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

internal sealed record GetCurrentPriceExceptionRequest(long CaseId, int SalesArrangementId)
    : IRequest<GetCurrentPriceExceptionResponse>
{
}
