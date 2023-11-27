using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.Product.GetCaseIdByPcpId;

internal class GetCaseIdByPcpIdHandler : IRequestHandler<GetCaseIdByPcpIdRequest, GetCaseIdByPcpIdResponse>
{
    private readonly IProductServiceClient _productService;

    public GetCaseIdByPcpIdHandler(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public async Task<GetCaseIdByPcpIdResponse> Handle(GetCaseIdByPcpIdRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new GetCaseIdRequest
        {
            PcpId = new PcpIdObject { PcpId = request.PcpId }
        };

        var response = await _productService.GetCaseId(dsRequest, cancellationToken);

        return new GetCaseIdByPcpIdResponse(response.CaseId);
    }
}