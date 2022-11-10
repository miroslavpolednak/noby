using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1._1;
using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients;


namespace DomainServices.ProductService.Api.Handlers;

internal class CreateMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Dto.CreateMortgageMediatrRequest, ProductIdReqRes>
{
    #region Construction

    private readonly ICaseServiceClient _caseService;

    public CreateMortgageHandler(
        ICodebookServiceClients codebookService,
        ICaseServiceClient caseService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateMortgageHandler> logger
       ) : base(codebookService, repository, mpHomeClient, logger)
    {
        _caseService = caseService;
    }

    #endregion

    public async Task<ProductIdReqRes> Handle(Dto.CreateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _ = ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.Request.CaseId, cancellation))
          ?? throw new CisNotFoundException(12000, $"Case ID #{request.Request.CaseId} does not exist.");

        await UpdateLoan(request.Request.CaseId, request.Request.Mortgage, true, cancellation);

        var model = new ProductIdReqRes
        {
           ProductId = request.Request.CaseId,
        };

        return model;
    }
  
}