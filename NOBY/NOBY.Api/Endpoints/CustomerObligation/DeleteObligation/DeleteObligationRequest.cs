namespace NOBY.Api.Endpoints.CustomerObligation.DeleteObligation;

internal record DeleteObligationRequest(int CustomerOnSAId, int ObligationId)
    : IRequest
{
}
