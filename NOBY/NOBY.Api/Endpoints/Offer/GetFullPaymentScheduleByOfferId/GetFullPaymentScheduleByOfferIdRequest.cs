using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal record GetFullPaymentScheduleByOfferIdRequest(int OfferId)
    : IRequest<Dto.GetFullPaymentScheduleResponse>
{ }
