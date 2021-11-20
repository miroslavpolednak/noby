using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Offer.Dto;
using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetBuildingSavingsDepositScheduleHandler 
    : IRequestHandler<GetBuildingSavingsDepositScheduleRequest, GetBuildingSavingsDepositScheduleResponse>
{
    private readonly IOfferServiceAbstraction _offerService;
    
    public GetBuildingSavingsDepositScheduleHandler(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<GetBuildingSavingsDepositScheduleResponse> Handle(GetBuildingSavingsDepositScheduleRequest request, CancellationToken cancellationToken)
    {
        var result = resolveResult(await _offerService.GetBuildingSavingsDepositSchedule(request.OfferInstanceId));
                
        return new GetBuildingSavingsDepositScheduleResponse(result.ScheduleItems.Select(t => (ScheduleItem)t));
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };
}
