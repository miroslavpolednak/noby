namespace DomainServices.CaseService.Api.Handlers;

internal class DeleteCaseHandler
    : IRequestHandler<Dto.DeleteCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteCaseMediatrRequest request, CancellationToken cancellation)
    {
        var count = ServiceCallResult
            .ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellation))
            .SalesArrangements.Count;

        if (count > 0)
            throw new CisValidationException(13021, "Unable to delete Case – one or more SalesArrangements exists for this case");

        // ulozit do DB
        await _repository.DeleteCase(request.CaseId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public DeleteCaseHandler(
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Repositories.CaseServiceRepository repository)
    {
        _repository = repository;
        _salesArrangementService = salesArrangementService;
    }
}
