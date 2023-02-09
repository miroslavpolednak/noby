using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseData;

internal sealed class UpdateCaseDataHandler
    : IRequestHandler<UpdateCaseDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductTypes(cancellation)).Any(t => t.Id == request.Data.ProductTypeId))
            throw new CisNotFoundException(13014, nameof(request.Data.ProductTypeId), request.Data.ProductTypeId);

        var bonusChanged = entity.IsEmployeeBonusRequested != request.Data.IsEmployeeBonusRequested;

        // ulozit do DB
        entity.ContractNumber = request.Data.ContractNumber;
        entity.TargetAmount = request.Data.TargetAmount;
        entity.IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested;
        entity.ProductTypeId = request.Data.ProductTypeId;

        await _dbContext.SaveChangesAsync(cancellation);

        // pokud se zmenil IsEmployeeBonusRequested, zavolat EAS
        if (bonusChanged)
        {
            await _mediator.Publish(new Notifications.CaseStateChangedNotification
            {
                CaseId = request.CaseId,
                CaseStateId = entity.State,
                ClientName = $"{entity.FirstNameNaturalPerson} {entity.Name}",
                ProductTypeId = request.Data.ProductTypeId,
                CaseOwnerUserId = entity.OwnerUserId
            }, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseDataHandler(
        IMediator mediator,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        CaseServiceDbContext dbContext)
    {
        _mediator = mediator;
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
