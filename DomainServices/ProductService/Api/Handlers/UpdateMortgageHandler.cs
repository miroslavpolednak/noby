using ExternalServices.MpHome.V1._1;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.ProductService.Api.Handlers;

internal class UpdateMortgageHandler
    : BaseMortgageHandler, IRequestHandler<Dto.UpdateMortgageMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public UpdateMortgageHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<UpdateMortgageHandler> logger) : base(codebookService, repository, mpHomeClient, logger){}

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(UpdateMortgageHandler));

        await UpdateLoan(request.Request.ProductId, request.Request.Mortgage, false, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
  
}