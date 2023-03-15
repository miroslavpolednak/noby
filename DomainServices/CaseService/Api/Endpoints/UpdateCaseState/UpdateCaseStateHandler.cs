using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseState;

internal sealed class UpdateCaseStateHandler
    : IRequestHandler<UpdateCaseStateRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseStateRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        int currentCaseState = entity.State;

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates(cancellation)).Any(t => t.Id == request.State))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseStateNotFound, request.State);
        }
        
        if (currentCaseState == request.State)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseStateAlreadySet);
        }

        // Zakázané přechody mezi stavy
        if ((currentCaseState == 6 || currentCaseState == 7)
            || (currentCaseState == 2 && request.State == 1))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseStateNotAllowed);

        // pokud je true, meli bychom poslat info SB se zmenou stavu
        bool shouldNotifySbAboutStateChange = request.StateUpdatedInStarbuild == UpdatedInStarbuildStates.Unknown && _starbuildStateUpdateStates.Contains(currentCaseState);

        // update v DB
        entity.StateUpdatedInStarbuild = (byte)request.StateUpdatedInStarbuild;
        entity.State = request.State;
        entity.StateUpdateTime = _dateTime.Now;

        await _dbContext.SaveChangesAsync(cancellation);

        // fire notification
        if (shouldNotifySbAboutStateChange)
        {
            await _mediator.Send(new NotifyStarbuildRequest
            {
                CaseId = request.CaseId
            }, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static int[] _starbuildStateUpdateStates = new[] { 1, 2, 7, 9 };

    private readonly CIS.Core.IDateTime _dateTime;
    private readonly IMediator _mediator;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseStateHandler(
        CIS.Core.IDateTime dateTime,
        IMediator mediator,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        CaseServiceDbContext dbContext)
    {
        _dateTime = dateTime;
        _mediator = mediator;
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}

