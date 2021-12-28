using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class UpdateCaseRequest
    : IRequest<SaveCaseResponse>, IValidatableRequest
{
    public int OfferInstanceId { get; init; }
    public int SalesArrangementId { get; init; }

    public UpdateCaseRequest(int offerInstanceId, int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        OfferInstanceId = offerInstanceId;
    }
}
