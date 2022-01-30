using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Create SA {type} for #{caseId}/#{offerInstanceId}", request.Request.SalesArrangementTypeId, request.Request.CaseId, request.Request.OfferInstanceId);

        // validace product instance type
        var salesArrangementType = (await _codebookService.SalesArrangementTypes()).FirstOrDefault(t => t.Id == request.Request.SalesArrangementTypeId)
            ?? throw new CisNotFoundException(16005, $"SalesArrangementType #{request.Request.SalesArrangementTypeId} does not exist.");

        // validace na existenci case
        var caseInstance = CIS.Core.Results.ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.Request.CaseId, cancellation))
            ?? throw new CisNotFoundException(16002, $"Case ID #{request.Request.CaseId} does not exist.");

        // validace na existenci offerInstance
        /*var offerInstance = CIS.Core.Results.ServiceCallResult.ResolveToDefault<OfferService.Contracts>(await _offerService.(request.Request.OfferInstanceId, cancellation))
            ?? throw new CisNotFoundException(16001, $"OfferInstance ID #{request.Request.OfferInstanceId} does not exist.");*/

        // get default case state
        int defaultSaState = (await _codebookService.SalesArrangementStates()).First(t => t.IsDefaultNewState).Id;

        // ulozit do DB
        var saEntity = new Repositories.Entities.SalesArrangement()
        {
            CaseId = request.Request.CaseId,
            SalesArrangementTypeId = request.Request.SalesArrangementTypeId,
            State = defaultSaState,
            StateUpdateTime = DateTime.Now,
            OfferInstanceId = request.Request.OfferInstanceId
        };
        var salesArrangementId = await _repository.CreateSalesArrangement(saEntity, cancellation);

        _logger.LogDebug("SalesArrangement ID #{id} saved", salesArrangementId);

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;

    public CreateSalesArrangementHandler(
        OfferService.Abstraction.IOfferServiceAbstraction offerService,
        CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _offerService = offerService;
        _caseService = caseService;
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
