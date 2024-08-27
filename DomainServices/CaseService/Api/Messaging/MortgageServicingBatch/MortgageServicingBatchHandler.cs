using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MortgageServicingBatch;

internal sealed class MortgageServicingBatchHandler(Database.CaseServiceDbContext dbContext, ILogger<MortgageServicingBatchHandler> logger) : IMessageMiddleware
{
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        var batch = context.GetMessagesBatch();

        Dictionary<long, decimal> parsedData = new(batch.Count);

        foreach (var messageContext in batch)
        {
            var data = messageContext.Message.Value switch
            {
                cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged mortgageInstance => GetMortgageInstanceData(mortgageInstance),
                cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged mortgageApplication => GetMortgageApplicationData(mortgageApplication),
                _ => default
            };

            if (data == default)
                continue;

            parsedData[data.caseId] = data.targetAmount;
        }

        var availableCaseIds = await dbContext.Cases.AsNoTracking().Where(c => parsedData.Keys.Contains(c.CaseId)).Select(c => c.CaseId).OrderBy(id => id).ToListAsync();

        foreach ((long caseId, decimal targetAmount) in parsedData)
        {
            if (availableCaseIds.BinarySearch(caseId) < 0)
                continue;

            var caseEntry = dbContext.Cases.Attach(new() { CaseId = caseId });

            caseEntry.Entity.TargetAmount = targetAmount;

            logger.KafkaMortgageChangedFinished(nameof(MortgageServicingBatchHandler), caseId, targetAmount);
        }

        await dbContext.SaveChangesAsync();
    }

    private (long caseId, decimal targetAmount) GetMortgageInstanceData(cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged mortgageInstance)
    {
        if (long.TryParse(mortgageInstance.New?.Starbuild?.id, out var caseId))
            return (caseId, (decimal)mortgageInstance.New.loanAmount.limit);

        logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageServicingBatchHandler), mortgageInstance.New?.Starbuild?.id ?? string.Empty);

        return default;
    }

    private (long caseId, decimal targetAmount) GetMortgageApplicationData(cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged mortgageApplication)
    {
        if (long.TryParse(mortgageApplication.New.MortgageInstanceRequested.Starbuild?.id, out var caseId))
            return (caseId, (decimal)mortgageApplication.New.MortgageInstanceRequested.loanAmount.limit);

        logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageServicingBatchHandler), mortgageApplication.New.MortgageInstanceRequested?.Starbuild?.id ?? string.Empty);

        return default;
    }
}