using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Clients;
using ExternalServices.MpHome.V1_1;

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
        await _caseService.GetCaseDetail(request.Request.CaseId, cancellation);

        await UpdateLoan(request.Request.CaseId, request.Request.Mortgage, true, cancellation);

        var model = new ProductIdReqRes
        {
           ProductId = request.Request.CaseId,
        };

        return model;
    }
  
}