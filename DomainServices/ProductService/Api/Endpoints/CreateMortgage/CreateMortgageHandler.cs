using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients;
using ExternalServices.MpHome.V1_1;
using DomainServices.ProductService.Api.Database;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageHandler
    : IRequestHandler<CreateMortgageRequest, CreateMortgageResponse>
{
    #region Construction

    private readonly ICodebookServiceClient _codebookService;
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;
    private readonly ICaseServiceClient _caseService;
    private readonly DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient _pcpClient;

    public CreateMortgageHandler(
        DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient pcpClient,
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService,
        LoanRepository repository,
        IMpHomeClient mpHomeClient)
    {
        _codebookService = codebookService;
        _repository = repository;
        _mpHomeClient = mpHomeClient;
        _pcpClient = pcpClient;
        _caseService = caseService;
    }

    #endregion

    public async Task<CreateMortgageResponse> Handle(Contracts.CreateMortgageRequest request, CancellationToken cancellation)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellation);

        if (await _repository.ExistsLoan(caseInstance.CaseId, cancellation))
        {
            throw new CisAlreadyExistsException(12005, nameof(caseInstance.CaseId), caseInstance.CaseId);
        }

        // create in pcp
        string? newPcpId = null;
        if (caseInstance.Customer?.Identity?.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
        {
            //var pcpId = (await _codebookService.ProductTypes(cancellation)).First(t => t.Id == request.Mortgage.ProductTypeId).PcpId;
            var pcpId = "220614142026340857"; //MOCK
            newPcpId = await _pcpClient.CreateProduct(request.CaseId, caseInstance.Customer.Identity.IdentityId, pcpId, cancellation);
        }

        // create in konsdb
        var mortgageRequest = request.Mortgage.ToMortgageRequest(newPcpId);
        await _mpHomeClient.UpdateLoan(caseInstance.CaseId, mortgageRequest, cancellation);

        return new CreateMortgageResponse
        {
            ProductId = request.CaseId,
        };
    }

}