namespace FOMS.Api.Endpoints.SalesArrangement.SendToCmp;

internal record SendToCmpRequest(int SalesArrangementId)
    : IRequest
{
}
