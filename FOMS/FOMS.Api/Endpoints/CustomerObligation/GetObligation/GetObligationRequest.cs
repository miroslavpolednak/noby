namespace FOMS.Api.Endpoints.CustomerObligation.GetObligation;

internal record GetObligationRequest(int SalesArrangementId, int ObligationId)
    : IRequest<Dto.ObligationFullDto>
{
}
