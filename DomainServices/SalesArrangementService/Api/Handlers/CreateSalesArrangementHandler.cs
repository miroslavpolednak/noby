using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.CreateSalesArrangementStarted(request.Request.SalesArrangementTypeId, request.Request.CaseId, request.Request.OfferId);

        // validace product instance type
        _ = (await _codebookService.SalesArrangementTypes()).FirstOrDefault(t => t.Id == request.Request.SalesArrangementTypeId)
            ?? throw new CisNotFoundException(16005, $"SalesArrangementTypeId #{request.Request.SalesArrangementTypeId} does not exist.");

        // validace na existenci case
        //TODO je nejaka spojitost mezi ProductTypeId a SalesArrangementTypeId, ktera by se dala zkontrolovat?
        _ = ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.Request.CaseId, cancellation))
            ?? throw new CisNotFoundException(16002, $"Case ID #{request.Request.CaseId} does not exist.");

        // validace na existenci offer
        if (request.Request.OfferId.HasValue)
            _ = ServiceCallResult.ResolveToDefault<DomainServices.OfferService.Contracts.GetOfferResponse>(await _offerService.GetOffer(request.Request.OfferId.Value, cancellation))
                ?? throw new CisNotFoundException(16001, $"Offer ID #{request.Request.OfferId} does not exist.");

        // get default SA state
        int defaultSaState = (await _codebookService.SalesArrangementStates()).First(t => t.IsDefault).Id;

        // ulozit do DB
        var saEntity = new Repositories.Entities.SalesArrangement()
        {
            CaseId = request.Request.CaseId,
            SalesArrangementTypeId = request.Request.SalesArrangementTypeId,
            State = defaultSaState,
            StateUpdateTime = DateTime.Now,
            OfferId = request.Request.OfferId
        };
        var salesArrangementId = await _repository.CreateSalesArrangement(saEntity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.SalesArrangement), salesArrangementId);

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
