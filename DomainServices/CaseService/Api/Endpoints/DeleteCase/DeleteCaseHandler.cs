using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.DeleteCase;

internal sealed class DeleteCaseHandler
    : IRequestHandler<DeleteCaseRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCaseRequest request, CancellationToken cancellation)
    {
        // case entity
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation) 
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);

        // pocet SA na Case
        var count = ServiceCallResult
            .ResolveAndThrowIfError<SalesArrangementService.Contracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellation))
            .SalesArrangements.Count;

        if (count > 0)
            throw new CisValidationException(13021, "Unable to delete Case – one or more SalesArrangements exists for this case");

        // ulozit do DB
        _dbContext.Cases.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public DeleteCaseHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _salesArrangementService = salesArrangementService;
    }
}
