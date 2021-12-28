using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class UpdateCaseRequest
    : IRequest<SaveCaseResponse>, IValidatableRequest
{
    public readonly CIS.Core.Types.CustomerIdentity? Customer;

    public string? CustomerScheme { get; init; }
    public int? CustomerId { get; init; }
    public int OfferInstanceId { get; init; }
    public int SalesArrangementId { get; init; }

    public UpdateCaseRequest(int offerInstanceId, int salesArrangementId, string? customerScheme, int? customerId)
    {
        SalesArrangementId = salesArrangementId;
        OfferInstanceId = offerInstanceId;
        Customer = new CIS.Core.Types.CustomerIdentity(customerId, customerScheme);
    }
}
