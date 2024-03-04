using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MortgageApplicationChanged;

internal sealed class MortgageApplicationChangedConsumer
    : IConsumer<cz.kb.api.mortgageservicingevents.v2.MortgageApplicationChanged>
{
    public async Task Consume(ConsumeContext<cz.kb.api.mortgageservicingevents.v2.MortgageApplicationChanged> context)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageApplicationChangedConsumer));

        if (long.TryParse(context.Message.New.MortgageInstanceRequested.Starbuild?.id, out long caseId))
        {
            var instance = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == caseId, context.CancellationToken);
            if (instance is not null)
            {
                instance.TargetAmount = (decimal)context.Message.New.MortgageInstanceRequested.loanAmount.limit;

                await _dbContext.SaveChangesAsync(context.CancellationToken);

                _logger.KafkaMortgageChangedFinished(nameof(MortgageApplicationChangedConsumer), caseId, instance.TargetAmount);
            }
            else
            {
                _logger.KafkaCaseIdNotFound(nameof(MortgageApplicationChangedConsumer), caseId);
            }
        }
        else
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageApplicationChangedConsumer), context.Message.New.MortgageInstanceRequested.Starbuild?.id ?? "");
        }
    }

    private readonly ILogger<MortgageApplicationChangedConsumer> _logger;
    private readonly Database.CaseServiceDbContext _dbContext;

    public MortgageApplicationChangedConsumer(Database.CaseServiceDbContext dbContext, ILogger<MortgageApplicationChangedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
