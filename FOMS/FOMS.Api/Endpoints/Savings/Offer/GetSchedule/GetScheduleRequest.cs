namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class GetScheduleRequest
    : IRequest<GetScheduleResponse>
{
    public int OfferInstanceId { get; set; }
    public DomainServices.OfferService.Contracts.ScheduleItemTypes ScheduleType { get; set; }

    public GetScheduleRequest(int offerInstanceId, int type)
    {
        OfferInstanceId = offerInstanceId;
        ScheduleType = type == 2 ? DomainServices.OfferService.Contracts.ScheduleItemTypes.PaymentSchedule : DomainServices.OfferService.Contracts.ScheduleItemTypes.DepositSchedule;
    }
}
