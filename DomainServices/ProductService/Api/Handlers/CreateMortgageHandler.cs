using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1._1;
using DomainServices.CodebookService.Abstraction;


namespace DomainServices.ProductService.Api.Handlers;

internal class CreateMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Dto.CreateMortgageMediatrRequest, ProductIdReqRes>
{
    #region Construction

    public CreateMortgageHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateMortgageHandler> logger):base(codebookService, repository, mpHomeClient, logger){}

    #endregion

    public async Task<ProductIdReqRes> Handle(Dto.CreateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateMortgageHandler));

        await UpdateLoan(request.Request.CaseId, request.Request.Mortgage, true, cancellation);

        var model = new ProductIdReqRes
        {
           ProductId = request.Request.CaseId,
        };

        return model;
    }
  
}