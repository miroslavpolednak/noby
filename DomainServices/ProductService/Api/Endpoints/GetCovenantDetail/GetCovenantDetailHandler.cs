using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return new GetCovenantDetailResponse
        {
            Covenant = new CovenantDetail
            {
                Description = "",
                Name = "",
                Text = "",
                FulfillDate = DateTime.Now,
                IsFulfilled = true
            }
        };
    }
}