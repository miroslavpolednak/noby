using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.Product.GetCaseIdByPcpId;

internal sealed class GetCaseIdByPcpIdHandler(IProductServiceClient _productService)
        : IRequestHandler<GetCaseIdByPcpIdRequest, ProductGetCaseIdByPcpIdResponse>
{
    public async Task<ProductGetCaseIdByPcpIdResponse> Handle(GetCaseIdByPcpIdRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new GetCaseIdRequest
        {
            PcpId = new PcpIdObject { PcpId = request.PcpId }
        };

        var response = await _productService.GetCaseId(dsRequest, cancellationToken);

        return new ProductGetCaseIdByPcpIdResponse { CaseId = response.CaseId };
    }
}