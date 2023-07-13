namespace NOBY.Api.Endpoints.Product.GetCaseIdByPcpId;

public record GetCaseIdByPcpIdRequest(string PcpId) : IRequest<GetCaseIdByPcpIdResponse>
{
}