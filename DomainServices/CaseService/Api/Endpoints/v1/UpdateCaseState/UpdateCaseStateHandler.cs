using CIS.Infrastructure.Caching.Grpc;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateCaseState;

internal sealed class UpdateCaseStateHandler(
    IMediator _mediator,
    IGrpcServerResponseCache _responseCache,
    CodebookService.Clients.ICodebookServiceClient _codebookService,
    CaseServiceDbContext _dbContext,
    TimeProvider _timeProvider)
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
        if (CaseHelpers.IsCaseInState([EnumCaseStates.Finished, EnumCaseStates.Cancelled], (EnumCaseStates)currentCaseState)
            || CaseHelpers.IsCaseInState([EnumCaseStates.InProgress], (EnumCaseStates)request.State))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.CaseStateNotAllowed);
        }

        // pokud je true, meli bychom poslat info SB se zmenou stavu
        bool shouldNotifySbAboutStateChange = request.StateUpdatedInStarbuild == UpdatedInStarbuildStates.Unknown && _starbuildStateUpdateStates.Contains(currentCaseState);

        // update v DB
        if (entity.State != request.State)
        {
            // zapisovat cas zmeny pouze pokud se fakticky zmeni stav
            entity.StateUpdateTime = _timeProvider.GetLocalNow().DateTime;
        }
        entity.StateUpdatedInStarbuild = (byte)request.StateUpdatedInStarbuild;
        entity.State = request.State;
        
        await _dbContext.SaveChangesAsync(cancellation);

        await _responseCache.InvalidateEntry(nameof(ValidateCaseId), request.CaseId);
        await _responseCache.InvalidateEntry(nameof(GetCaseDetail), request.CaseId);

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

    private static readonly int[] _starbuildStateUpdateStates = [1, 2, 7, 9];
}

