using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal class GetBuildingSavingsScheduleRequest
    : IRequest<GetBuildingSavingsScheduleResponse>, CIS.Core.Validation.IValidatableRequest
{
    public ScheduleItemTypes ScheduleType { get; init; }
    public int OfferInstanceId { get; init; }

    public GetBuildingSavingsScheduleRequest(int offerInstanceId, ScheduleItemTypes scheduleType)
    {
        ScheduleType = scheduleType;
        OfferInstanceId = offerInstanceId;
    }
}
