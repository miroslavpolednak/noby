using CIS.Core;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.ExternalServices.Pcp;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgagePcpId;

internal sealed class UpdateMortgagePcpIdHandler(
    IConfiguration _configuration,
    IMpHomeClient _mpHomeClient,
    ICodebookServiceClient _codebookService,
    IPcpClient _pcpClient)
    : IRequestHandler<UpdateMortgagePcpIdRequest, UpdateMortgagePcpIdResponse>
{
    public async Task<UpdateMortgagePcpIdResponse> Handle(UpdateMortgagePcpIdRequest request, CancellationToken cancellationToken)
    {
        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var pcpCurrentVersion = _configuration[$"{CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ExternalServices.Pcp.IPcpClient.ServiceName}:VersionInUse"];

        string pcpProductIdOrObjectCode = pcpCurrentVersion switch
        {
            ExternalServices.Pcp.IPcpClient.Version => productTypes.First(t => t.Id == request.ProductTypeId).PcpProductId,
            ExternalServices.Pcp.IPcpClient.Version2 => productTypes.First(t => t.Id == request.ProductTypeId).PcpObjectCode,
            _ => throw new ArgumentException("Not implemented version")
        };

        var pcpId = await _pcpClient.CreateProduct(request.ProductId, request.Identity.IdentityId, pcpProductIdOrObjectCode, cancellationToken);

        // create in konsdb
        await _mpHomeClient.UpdatePcpId(request.ProductId, pcpId, cancellationToken);

        return new UpdateMortgagePcpIdResponse
        {
            PcpId = pcpId
        };
    }
}
