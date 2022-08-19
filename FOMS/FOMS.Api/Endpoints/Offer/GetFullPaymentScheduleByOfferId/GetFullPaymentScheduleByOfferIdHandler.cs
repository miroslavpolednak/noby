using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Offer.Dto;
using Microsoft.Azure.Services.AppAuthentication;
using DSContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal class GetFullPaymentScheduleByOfferIdHandler
    : IRequestHandler<GetFullPaymentScheduleByOfferIdRequest, Dto.GetFullPaymentScheduleResponse>
{
    public async Task<Dto.GetFullPaymentScheduleResponse> Handle(GetFullPaymentScheduleByOfferIdRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DSContracts.GetMortgageOfferFPScheduleResponse>(await _offerService.GetMortgageOfferFPSchedule(request.OfferId, cancellationToken));

        _logger.RequestHandlerFinished(nameof(GetFullPaymentScheduleByOfferIdHandler));

        var items = result.PaymentScheduleFull.Select(i =>
            new PaymentScheduleFullItem
            {
                PaymentNumber = i.PaymentNumber,
                Date = i.Date,
                Amount = i.Amount,
                Principal = i.Principal,
                Interest = i.Interest,
                RemainingPrincipal = i.RemainingPrincipal,
            }
        ).ToList();

        return new GetFullPaymentScheduleResponse
        {
            Items = items
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetFullPaymentScheduleByOfferIdHandler> _logger;

    public GetFullPaymentScheduleByOfferIdHandler(IOfferServiceAbstraction offerService, ILogger<GetFullPaymentScheduleByOfferIdHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
