namespace NOBY.Api.Endpoints.Product.GetCaseIdByPcpId;

internal sealed record GetCaseIdByPcpIdRequest(string PcpId) 
    : IRequest<ProductGetCaseIdByPcpIdResponse>
{
}