using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageHandler
    : BaseMortgageHandler, IRequestHandler<CreateMortgageRequest, CreateMortgageResponse>
{
    #region Construction

    private readonly ICaseServiceClient _caseService;

    public CreateMortgageHandler(
        ICodebookServiceClients codebookService,
        ICaseServiceClient caseService,
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateMortgageHandler> logger
       ) : base(codebookService, repository, mpHomeClient, logger)
    {
        _caseService = caseService;
    }

    #endregion

    public async Task<CreateMortgageResponse> Handle(Contracts.CreateMortgageRequest request, CancellationToken cancellation)
    {
        await _caseService.GetCaseDetail(request.CaseId, cancellation);

        await UpdateLoan(request.CaseId, request.Mortgage, true, cancellation);

        return new CreateMortgageResponse
        {
            ProductId = request.CaseId,
        };
    }

}