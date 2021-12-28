using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCaseRequest
    : IRequest<SaveCaseResponse>, IValidatableRequest
{
    public readonly CIS.Core.Types.CustomerIdentity? Customer;

    public string? CustomerScheme { get; init; }
    public int? CustomerId { get; init; }
    public int OfferInstanceId { get; init; }

    public CreateCaseRequest(int offerInstanceId, string? customerScheme, int? customerId)
    {
        OfferInstanceId = offerInstanceId;
        Customer = new CIS.Core.Types.CustomerIdentity(customerId, customerScheme);
    }
}
