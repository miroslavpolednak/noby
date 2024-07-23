using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal sealed class GetFullPaymentScheduleByOfferIdHandler(IOfferServiceClient _offerService)
        : IRequestHandler<GetFullPaymentScheduleByOfferIdRequest, OfferGetFullPaymentScheduleResponse>
{
    public async Task<OfferGetFullPaymentScheduleResponse> Handle(GetFullPaymentScheduleByOfferIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _offerService.GetMortgageOfferFPSchedule(request.OfferId, cancellationToken);

            return new()
            {
                Items = result.PaymentScheduleFull.Select(i =>
                    new OfferGetFullPaymentScheduleItem
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
}
