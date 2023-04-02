using CIS.Infrastructure.Caching;
using DomainServices.CaseService.Api.Database;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.CaseService.Api.Messaging.CaseStateChangedProcessingCompleted;

internal sealed class CaseStateChanged_ProcessingCompletedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.CaseStateChanged_ProcessingCompleted>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CaseStateChanged_ProcessingCompletedConsumer> _logger;

    public CaseStateChanged_ProcessingCompletedConsumer(CaseServiceDbContext dbContext, IDistributedCache distributedCache, ILogger<CaseStateChanged_ProcessingCompletedConsumer> logger)
    {
        _distributedCache = distributedCache;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.CaseStateChanged_ProcessingCompleted> context)
    {
        var cache = await _distributedCache.GetObjectAsync<SharedDto.CaseStateChangeRequestId>($"CaseStateChanged_{context.Message.workflowInputProcessingContext.requestId}");
        if (cache is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RequestNotFoundInCache, context.Message.workflowInputProcessingContext.requestId);
        }

        var entity = _dbContext.Cases
            .FirstOrDefault(t => t.CaseId == cache.CaseId)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, cache.CaseId);

        if (context.Message.workflowInputProcessingContext.requestProcessingResult == cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.RequestProcessingResultEnum.OK )
        {
            entity.State = cache.CaseState;
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Ok;
        }
        else
        {
            entity.State = cache.CaseState;
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Error;
            
            _logger.StarbuildStateUpdateFailed(cache.CaseId, cache.CaseState);
        }
        
        _dbContext.SaveChanges();
    }
}
