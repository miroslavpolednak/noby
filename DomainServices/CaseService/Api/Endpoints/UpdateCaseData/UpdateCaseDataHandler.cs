using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseData;

internal sealed class UpdateCaseDataHandler
    : IRequestHandler<UpdateCaseDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext
            .Cases
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductTypes(cancellation)).Any(t => t.Id == request.Data.ProductTypeId))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ProductTypeIdNotFound, request.Data.ProductTypeId);
        }

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
            try
            {
                await _mediator.Send(new NotifyStarbuildRequest
                {
                    CaseId = request.CaseId
                }, cancellation);
            }
            catch (Exception ex)
            {
                // pouze logujeme!
                _logger.CaseStateChangedFailed(request.CaseId, ex);
            }
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ILogger<UpdateCaseDataHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseDataHandler(
        ILogger<UpdateCaseDataHandler> logger,
        IMediator mediator,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        CaseServiceDbContext dbContext)
    {
        _logger = logger;
        _mediator = mediator;
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
