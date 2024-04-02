﻿using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using SharedTypes.Enums;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateCaseState;

internal sealed class UpdateCaseStateHandler
    : IRequestHandler<UpdateCaseStateRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseStateRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        int currentCaseState = entity.State;

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates(cancellation)).Any(t => t.Id == request.State))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseStateNotFound, request.State);
        }

        // tento stav jiz Case ma
        if (currentCaseState == request.State)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.CaseStateAlreadySet);
        }

        // Zakázané přechody mezi stavy
        if (currentCaseState == (int)CaseStates.Finished
            || currentCaseState == (int)CaseStates.Cancelled
            || request.State == (int)CaseStates.InProgress)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.CaseStateNotAllowed);
        }

        // pokud je true, meli bychom poslat info SB se zmenou stavu
        bool shouldNotifySbAboutStateChange = request.StateUpdatedInStarbuild == UpdatedInStarbuildStates.Unknown && _starbuildStateUpdateStates.Contains(currentCaseState);

        // update v DB
        entity.StateUpdatedInStarbuild = (byte)request.StateUpdatedInStarbuild;
        entity.State = request.State;
        entity.StateUpdateTime = _timeProvider.GetLocalNow().DateTime;

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

    private readonly TimeProvider _timeProvider;
    private readonly IMediator _mediator;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseStateHandler(
        IMediator mediator,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        CaseServiceDbContext dbContext,
        TimeProvider timeProvider)
    {
        _mediator = mediator;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }
}
