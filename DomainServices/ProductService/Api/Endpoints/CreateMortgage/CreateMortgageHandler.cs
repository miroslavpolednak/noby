using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageHandler : IRequestHandler<CreateMortgageRequest, CreateMortgageResponse>
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;
    private readonly ICaseServiceClient _caseService;
    private readonly ExternalServices.Pcp.V1.IPcpClient _pcpClient;

    public CreateMortgageHandler(
        ExternalServices.Pcp.V1.IPcpClient pcpClient,
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

    public async Task<CreateMortgageResponse> Handle(CreateMortgageRequest request, CancellationToken cancellationToken)
    {
        if (await _repository.LoanExists(request.CaseId, cancellationToken))
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.AlreadyExists12005, request.CaseId);

        // create in pcp
        var pcpId = await CreatePcpId(request, cancellationToken);

        // create in konsdb
        await _mpHomeClient.UpdateLoan(request.CaseId, request.Mortgage.ToMortgageRequest(pcpId), cancellationToken);

        return new CreateMortgageResponse { ProductId = request.CaseId };
    }

    private async Task<string?> CreatePcpId(CreateMortgageRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        if (caseInstance.Customer?.Identity?.IdentityScheme != SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)
            return default;

        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var pcpProductId = productTypes.First(t => t.Id == request.Mortgage.ProductTypeId).PcpProductId;

        return await _pcpClient.CreateProduct(request.CaseId, caseInstance.Customer.Identity.IdentityId, pcpProductId, cancellationToken);

    }
}