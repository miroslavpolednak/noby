namespace FOMS.Api.Endpoints.Product.GetCustomersOnProduct;

internal sealed record GetCustomersOnProductRequest(long CaseId)
    : IRequest<List<GetCustomersOnProductCustomer>>
{
}
