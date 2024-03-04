using CIS.Infrastructure.Caching;
using cz.mpss.api.starbuild.mortgageworkflow.mortgageinputprocessingevents.v1;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Messaging.CaseStateChangedProcessingCompleted;
using KafkaFlow;
using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class CaseStateChangedProcessingCompletedHandler : IMessageHandler<CaseStateChanged_ProcessingCompleted>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CaseStateChanged_ProcessingCompletedConsumer> _logger;

    public CaseStateChangedProcessingCompletedHandler(CaseServiceDbContext dbContext, IDistributedCache distributedCache, ILogger<CaseStateChanged_ProcessingCompletedConsumer> logger)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, CaseStateChanged_ProcessingCompleted message)
    {
        var cache = await _distributedCache.GetObjectAsync<SharedDto.CaseStateChangeRequestId>($"CaseStateChanged_{message.workflowInputProcessingContext.requestId}");
        if (cache is null)
        {
            _logger.RequestNotFoundInCache(message.workflowInputProcessingContext.requestId);
            return;
        }

        var entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == cache.CaseId);

        if (entity is null)
        {
            _logger.KafkaCaseIdNotFound(nameof(CaseStateChanged_ProcessingCompletedConsumer), cache.CaseId);
            return;
        }

        if (message.workflowInputProcessingContext.requestProcessingResult == RequestProcessingResultEnum.OK)
        {
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Ok;

            _logger.StarbuildStateUpdateSuccess(cache.CaseId, cache.RequestId);
        }
        else
        {
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Error;

            _logger.StarbuildStateUpdateFailed(cache.CaseId, cache.RequestId);
        }

        await _dbContext.SaveChangesAsync();
    }
}