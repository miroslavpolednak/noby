using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.Product.GetCaseIdByPcpId;

internal class GetCaseIdByPcpIdHandler : IRequestHandler<GetCaseIdByPcpIdRequest, GetCaseIdByPcpIdResponse>
{
    private readonly IProductServiceClient _productService;
    private readonly ICaseServiceClient _caseService;

    public GetCaseIdByPcpIdHandler(IProductServiceClient productService, ICaseServiceClient caseService)
    {
        _productService = productService;
        _caseService = caseService;
    }

    public async Task<GetCaseIdByPcpIdResponse> Handle(GetCaseIdByPcpIdRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new GetCaseIdRequest
        {
            PcpId = new PcpIdObject { PcpId = request.PcpId }
        };

        var response = await _productService.GetCaseId(dsRequest, cancellationToken);

        var validateCaseResponse = await _caseService.ValidateCaseId(response.CaseId, cancellationToken: cancellationToken);

        if (!validateCaseResponse.Exists)
            throw new NobyValidationException(90026);

        return new GetCaseIdByPcpIdResponse(response.CaseId);
    }
}