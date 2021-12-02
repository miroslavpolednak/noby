using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;
using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class GetScheduleHandler 
    : IRequestHandler<GetScheduleRequest, GetScheduleResponse>
{
    private readonly IOfferServiceAbstraction _offerService;
    
    public GetScheduleHandler(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<GetScheduleResponse> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
    {
        DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse result;

        if (request.ScheduleType == DomainServices.OfferService.Contracts.ScheduleItemTypes.DepositSchedule)
            result = resolveResult(await _offerService.GetBuildingSavingsDepositSchedule(request.OfferInstanceId));
        else
            result = resolveResult(await _offerService.GetBuildingSavingsPaymentSchedule(request.OfferInstanceId));

        return new GetScheduleResponse(result.ScheduleItems.Select(t => (ScheduleItem)t));
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };
}
