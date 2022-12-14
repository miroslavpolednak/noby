using DomainServices.OfferService.Clients;
using NOBY.Api.Endpoints.Offer.Dto;

namespace NOBY.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal class GetFullPaymentScheduleByOfferIdHandler
    : IRequestHandler<GetFullPaymentScheduleByOfferIdRequest, Dto.GetFullPaymentScheduleResponse>
{
    public async Task<Dto.GetFullPaymentScheduleResponse> Handle(GetFullPaymentScheduleByOfferIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetMortgageOfferFPSchedule(request.OfferId, cancellationToken);

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

    private readonly IOfferServiceClient _offerService;

    public GetFullPaymentScheduleByOfferIdHandler(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }
}
