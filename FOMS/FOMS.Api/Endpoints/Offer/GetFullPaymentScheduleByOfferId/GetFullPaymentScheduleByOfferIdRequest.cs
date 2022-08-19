using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal record GetFullPaymentScheduleByOfferIdRequest(int OfferId)
    : IRequest<Dto.GetFullPaymentScheduleResponse>
{ }
