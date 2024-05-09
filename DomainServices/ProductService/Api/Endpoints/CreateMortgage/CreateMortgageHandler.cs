using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients.v1;
using CIS.Core;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageHandler(
    ExternalServices.Pcp.IPcpClient _pcpClient,
    ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    IMpHomeClient _mpHomeClient,
    IConfiguration _configuration) 
    : IRequestHandler<CreateMortgageRequest, CreateMortgageResponse>
{
    public async Task<CreateMortgageResponse> Handle(CreateMortgageRequest request, CancellationToken cancellationToken)
    {
        if (await _mpHomeClient.CaseExists(request.CaseId, cancellationToken))
        {
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.AlreadyExists12005, request.CaseId);
        }

        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        string? pcpId = null;

        if (caseInstance.Customer?.Identity?.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)
        {
            // create in pcp
            var productTypes = await _codebookService.ProductTypes(cancellationToken);

            var pcpCurrentVersion = _configuration[$"{CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ExternalServices.Pcp.IPcpClient.ServiceName}:VersionInUse"];

            string pcpProductIdOrObjectCode = pcpCurrentVersion switch
            {
                ExternalServices.Pcp.IPcpClient.Version => productTypes.First(t => t.Id == request.Mortgage.ProductTypeId).PcpProductId,
                ExternalServices.Pcp.IPcpClient.Version2 => productTypes.First(t => t.Id == request.Mortgage.ProductTypeId).PcpObjectCode,
                _ => throw new ArgumentException("Not implemented version")
            };

            pcpId = await _pcpClient.CreateProduct(request.CaseId, caseInstance.Customer.Identity.IdentityId, pcpProductIdOrObjectCode, cancellationToken);

            // create in konsdb
            await _mpHomeClient.UpdateLoan(request.CaseId, request.Mortgage.ToMortgageRequest(pcpId), cancellationToken);
        }

        return new CreateMortgageResponse
        {
            ProductId = request.CaseId,
            PcpId = pcpId
        };
    }
}