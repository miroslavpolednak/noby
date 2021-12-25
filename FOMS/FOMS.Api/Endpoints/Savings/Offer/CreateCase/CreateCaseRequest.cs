using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCaseRequest
    : IRequest<SaveCaseResponse>, IValidatableRequest
{
    public CIS.Core.Types.CustomerIdentity? Customer { get; init; }
    public int OfferInstanceId { get; init; }
}
