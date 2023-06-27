namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

internal sealed record GetCurrentPriceExceptionRequest(long CaseId)
    : IRequest<GetCurrentPriceExceptionResponse>
{
}
