namespace NOBY.Api.Endpoints.CustomerObligation.DeleteObligation;

internal sealed record DeleteObligationRequest(int CustomerOnSAId, int ObligationId)
    : IRequest
{
}
