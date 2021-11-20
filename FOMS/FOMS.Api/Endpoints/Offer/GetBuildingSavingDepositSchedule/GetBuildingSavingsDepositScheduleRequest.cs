namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed record GetBuildingSavingsDepositScheduleRequest(int OfferInstanceId)
    : IRequest<GetBuildingSavingsDepositScheduleResponse>
{
}
