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

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates(cancellation)).Any(t => t.Id == request.State))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseStateNotFound, request.State);
        }
        
        if (entity.State == request.State)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseStateAlreadySet);
        }

        // Zakázané přechody mezi stavy
        if (entity.State == 6 || entity.State == 2 && request.State == 1)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseStateNotAllowed);
        }

        // update v DB
        entity.State = request.State;
        entity.StateUpdateTime = _dateTime.Now;

        await _dbContext.SaveChangesAsync(cancellation);

        // fire notification
        if (entity.State == 1)
        {
            await _mediator.Publish(new Notifications.CaseStateChangedNotification
            {
                CaseId = request.CaseId,
                CaseStateId = request.State,
                ClientName = $"{entity.FirstNameNaturalPerson} {entity.Name}",
                ProductTypeId = entity.ProductTypeId,
                CaseOwnerUserId = entity.OwnerUserId,
                ContractNumber = entity.ContractNumber,
                IsEmployeeBonusRequested = entity.IsEmployeeBonusRequested
            }, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

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

