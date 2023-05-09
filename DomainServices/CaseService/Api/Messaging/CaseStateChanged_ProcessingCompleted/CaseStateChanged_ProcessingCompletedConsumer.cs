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
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.CaseStateChanged_ProcessingCompleted> context)
    {
        var cache = await _distributedCache.GetObjectAsync<SharedDto.CaseStateChangeRequestId>($"CaseStateChanged_{context.Message.workflowInputProcessingContext.requestId}");
        if (cache is null)
        {
            _logger.RequestNotFoundInCache(context.Message.workflowInputProcessingContext.requestId);
            return;
        }

        var entity = (await _dbContext.Cases
            .FirstOrDefaultAsync(t => t.CaseId == cache.CaseId, context.CancellationToken));

        if (entity is null)
        {
            _logger.KafkaCaseIdNotFound(cache.CaseId);
            return;
        }

        if (context.Message.workflowInputProcessingContext.requestProcessingResult == cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.RequestProcessingResultEnum.OK)
        {
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Ok;

            _logger.StarbuildStateUpdateSuccess(cache.CaseId, cache.RequestId);
        }
        else
        {
            entity.StateUpdatedInStarbuild = (byte)Contracts.UpdatedInStarbuildStates.Error;

            _logger.StarbuildStateUpdateFailed(cache.CaseId, cache.RequestId);
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
