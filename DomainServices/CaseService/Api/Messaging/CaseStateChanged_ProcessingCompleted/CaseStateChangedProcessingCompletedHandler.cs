using CIS.Infrastructure.Caching;
using cz.mpss.api.starbuild.mortgageworkflow.mortgageinputprocessingevents.v1;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using KafkaFlow;
using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class CaseStateChangedProcessingCompletedHandler(
    CaseServiceDbContext _dbContext, 
    IDistributedCache _distributedCache, 
    ILogger<CaseStateChangedProcessingCompletedHandler> _logger) 
    : IMessageHandler<CaseStateChanged_ProcessingCompleted>
{
	public async Task Handle(IMessageContext context, CaseStateChanged_ProcessingCompleted message)
    {
        var cache = await _distributedCache.GetObjectAsync<SharedDto.CaseStateChangeRequestId>($"CaseStateChanged_{message.workflowInputProcessingContext.requestId}");
        if (cache is null)
        {
            _logger.RequestNotFoundInCache(message.workflowInputProcessingContext.requestId);
            return;
        }

        UpdatedInStarbuildStates state;
        if (message.workflowInputProcessingContext.requestProcessingResult == RequestProcessingResultEnum.OK)
        {
            state = UpdatedInStarbuildStates.Ok;
            _logger.StarbuildStateUpdateSuccess(cache.CaseId, cache.RequestId);
        }
        else
        {
            state = UpdatedInStarbuildStates.Error;
            _logger.StarbuildStateUpdateFailed(cache.CaseId, cache.RequestId);
        }

        int updatedRows = await _dbContext
            .Cases
            .Where(t => t.CaseId == cache.CaseId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.StateUpdatedInStarbuild, (byte)state));

        if (updatedRows == 0)
        {
            _logger.KafkaCaseIdNotFound(nameof(CaseStateChangedProcessingCompletedHandler), cache.CaseId);
            return;
        }
    }
}