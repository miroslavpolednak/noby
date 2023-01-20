using DomainServices.OfferService.Clients;
using NOBY.Api.Endpoints.Offer.Dto;

namespace NOBY.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal sealed class GetFullPaymentScheduleByOfferIdHandler
    : IRequestHandler<GetFullPaymentScheduleByOfferIdRequest, Dto.GetFullPaymentScheduleResponse>
{
    public async Task<Dto.GetFullPaymentScheduleResponse> Handle(GetFullPaymentScheduleByOfferIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _offerService.GetMortgageOfferFPSchedule(request.OfferId, cancellationToken);

            return new GetFullPaymentScheduleResponse
            {
                Items = result.PaymentScheduleFull.Select(i =>
                    new PaymentScheduleFullItem
                    {
                        PaymentNumber = i.PaymentNumber,
                        Date = i.Date,
                        Amount = i.Amount,
                        Principal = i.Principal,
                        Interest = i.Interest,
                        RemainingPrincipal = i.RemainingPrincipal,
                    }
                ).ToList()
            };
        }
        catch (CisArgumentException ex)
        {
            // rethrow to be catched by validation middleware
            throw new CisValidationException(ex.ExceptionCode, ex.Message);
        }
    }

    private readonly IOfferServiceClient _offerService;

    public GetFullPaymentScheduleByOfferIdHandler(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }
}
