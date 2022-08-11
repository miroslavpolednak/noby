namespace DomainServices.CaseService.Api.Handlers;

internal class DeleteCaseHandler
    : IRequestHandler<Dto.DeleteCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteCaseMediatrRequest request, CancellationToken cancellation)
    {
        var count = ServiceCallResult
            .ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, null, cancellation))
            .SalesArrangements.Count;
        if (count > 0)
            throw new CisValidationException(0, "One or more SalesArrangements exists for this case"); //TODO: ErrorCode

        // ulozit do DB
        await _repository.DeleteCase(request.CaseId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public DeleteCaseHandler(
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        Repositories.CaseServiceRepository repository)
    {
        _repository = repository;
        _salesArrangementService = salesArrangementService;
    }
}
