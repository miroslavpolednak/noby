using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.ExternalServices.Pcp;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgagePcpId;

internal sealed class UpdateMortgagePcpIdHandler(
    IConfiguration _configuration,
    IMpHomeClient _mpHomeClient,
    ICaseServiceClient _caseService,
    ICodebookServiceClient _codebookService,
    IPcpClient _pcpClient,
    IMediator _mediator)
    : IRequestHandler<UpdateMortgagePcpIdRequest, UpdateMortgagePcpIdResponse>
{
    public async Task<UpdateMortgagePcpIdResponse> Handle(UpdateMortgagePcpIdRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.ProductId, cancellationToken);

        if (caseInstance.Customer?.Identity?.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)
        {
            // instance hypo
            var mortgage = await _mediator.Send(new GetMortgageRequest() { ProductId = request.ProductId }, cancellationToken);

            var productTypes = await _codebookService.ProductTypes(cancellationToken);
            var pcpCurrentVersion = _configuration[$"{CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ExternalServices.Pcp.IPcpClient.ServiceName}:VersionInUse"];

            string pcpProductIdOrObjectCode = pcpCurrentVersion switch
            {
                ExternalServices.Pcp.IPcpClient.Version => productTypes.First(t => t.Id == mortgage.Mortgage.ProductTypeId).PcpProductId,
                ExternalServices.Pcp.IPcpClient.Version2 => productTypes.First(t => t.Id == mortgage.Mortgage.ProductTypeId).PcpObjectCode,
                _ => throw new ArgumentException("Not implemented version")
            };

            var pcpId = await _pcpClient.CreateProduct(request.ProductId, caseInstance.Customer.Identity.IdentityId, pcpProductIdOrObjectCode, cancellationToken);

            // create in konsdb
            await _mpHomeClient.UpdateLoan(request.ProductId, mortgage.Mortgage.ToMortgageRequest(pcpId), cancellationToken);

            return new UpdateMortgagePcpIdResponse
            {
                PcpId = pcpId,
            };
        }
        else
        {
            return new UpdateMortgagePcpIdResponse();
        }
    }
}
