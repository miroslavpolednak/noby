namespace NOBY.Api.Endpoints.CustomerObligation.GetObligation;

internal sealed record GetObligationRequest(int SalesArrangementId, int ObligationId)
    : IRequest<SharedDto.ObligationFullDto>
{
}
