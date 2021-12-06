using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;
using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class GetScheduleHandler 
    : IRequestHandler<GetScheduleRequest, GetScheduleResponse>
{
    public async Task<GetScheduleResponse> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get schedule for {id}", request.OfferInstanceId);

        DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse result;

        if (request.ScheduleType == DomainServices.OfferService.Contracts.ScheduleItemTypes.DepositSchedule)
            result = resolveResult(await _offerService.GetBuildingSavingsDepositSchedule(request.OfferInstanceId));
        else
            result = resolveResult(await _offerService.GetBuildingSavingsPaymentSchedule(request.OfferInstanceId));

        _logger.LogDebug("Resolved {count} schedule items", result.ScheduleItems.Count());

        return new GetScheduleResponse(result.ScheduleItems.Select(t => (ScheduleItem)t));
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsScheduleResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetScheduleHandler> _logger;

    public GetScheduleHandler(IOfferServiceAbstraction offerService, ILogger<GetScheduleHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
