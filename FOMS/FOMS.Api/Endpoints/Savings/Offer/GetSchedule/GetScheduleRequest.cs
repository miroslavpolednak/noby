namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal sealed class GetScheduleRequest
    : IRequest<GetScheduleResponse>
{
    public int OfferInstanceId { get; init; }
    public DomainServices.OfferService.Contracts.ScheduleItemTypes ScheduleType { get; init; }

    public GetScheduleRequest(int offerInstanceId, int type)
    {
        OfferInstanceId = offerInstanceId;
        ScheduleType = type == 2 ? DomainServices.OfferService.Contracts.ScheduleItemTypes.PaymentSchedule : DomainServices.OfferService.Contracts.ScheduleItemTypes.DepositSchedule;
    }
}
