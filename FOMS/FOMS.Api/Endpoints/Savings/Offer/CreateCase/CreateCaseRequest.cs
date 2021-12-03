using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCaseRequest
    : IRequest<int>, IValidatableRequest
{
    public bool CreateProduct { get; init; }
    public CIS.Core.Types.CustomerIdentity? Customer { get; init; }
    public int OfferInstanceId { get; init; }
    public CreateCase Request { get; init; }

    public CreateCaseRequest(int offerInstanceId, CreateCase request, bool createProduct)
    {
        CreateProduct = createProduct;
        Request = request;
        OfferInstanceId = offerInstanceId;
        if (!string.IsNullOrEmpty(request.CustomerIdentity))
            Customer = new CIS.Core.Types.CustomerIdentity(request.CustomerIdentity);
    }
}
