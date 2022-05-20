namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseStateHandler
    : IRequestHandler<Dto.UpdateCaseStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseStateMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var caseInstance = await _repository.GetCaseDetail(request.CaseId, cancellation);

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates(cancellation)).Any(t => t.Id == request.State))
            throw new CisNotFoundException(13011, nameof(request.State), request.State);

        if (caseInstance.State == request.State)
            throw new CisValidationException(0, "Case state already set to the same value");
        // Zakázané přechody mezi stavy
        if (caseInstance.State == 6 || (caseInstance.State == 2 && request.State == 1))
            throw new CisValidationException(0, "Case state change not allowed");

        // update v DB
        await _repository.UpdateCaseState(request.CaseId, request.State, cancellation);

        // fire notification
        if (caseInstance.State == 1)
            await _mediator.Publish(new Notifications.CaseStateChangedNotification
            {
                CaseId = request.CaseId,
                CaseStateId = request.State,
                ClientName = $"{caseInstance.Customer?.FirstNameNaturalPerson} {caseInstance.Customer?.Name}",
                ProductTypeId = caseInstance.Data.ProductTypeId,
                CaseOwnerUserId = caseInstance.CaseOwner.UserId,
                ContractNumber = caseInstance.Data.ContractNumber
            }, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.CaseServiceRepository _repository;

    public UpdateCaseStateHandler(
        IMediator mediator,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CaseServiceRepository repository)
    {
        _mediator = mediator;
        _codebookService = codebookService;
        _repository = repository;
    }
}

