namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed record SendToCmpRequest(int SalesArrangementId, bool IgnoreWarnings)
    : IRequest<IActionResult>
{
}
