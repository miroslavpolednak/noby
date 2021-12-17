using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Create SA {type} for #{caseId}", request.SalesArrangementType, request.CaseId);

        //var caseInstance = await _caseService.GetCaseDetail(request.CaseId);
        //TODO nejaka validace na case?

        // get default case state
        int saState = request.State.HasValue ? request.State.Value : (await _codebookService.CaseStates()).First(t => t.IsDefaultNewState).Id;

        var salesArrangementId = await _repository.CreateSalesArrangement(new()
        {
            CaseId = request.CaseId,
            SalesArrangementType = request.SalesArrangementType,
            State = saState,
            OfferInstanceId = request.OfferInstanceId
        });

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;

    public CreateSalesArrangementHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
