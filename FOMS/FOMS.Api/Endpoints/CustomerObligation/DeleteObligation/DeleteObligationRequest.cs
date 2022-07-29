namespace FOMS.Api.Endpoints.CustomerObligation.DeleteObligation;

internal record DeleteObligationRequest(int CustomerOnSAId, int ObligationId)
    : IRequest
{
}
