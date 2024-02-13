using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MortgageInstanceChanged;

internal sealed class MortgageInstanceChangedConsumer
    : IConsumer<cz.kb.api.mortgageservicingevents.v2.MortgageInstanceChanged>
{
    public async Task Consume(ConsumeContext<cz.kb.api.mortgageservicingevents.v2.MortgageInstanceChanged> context)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageInstanceChangedConsumer));
        
        if (long.TryParse(context.Message.New.Starbuild.id, out long caseId))
        {
            var instance = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == caseId, context.CancellationToken);
            if (instance is not null)
            {
                instance.TargetAmount = (decimal)context.Message.New.loanAmount.limit;

                await _dbContext.SaveChangesAsync(context.CancellationToken);

                _logger.KafkaMortgageChangedFinished(nameof(MortgageInstanceChangedConsumer), caseId, instance.TargetAmount);
            }
            else
            {
                _logger.KafkaCaseIdNotFound(nameof(MortgageInstanceChangedConsumer), caseId);
            }
        }
        else
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageInstanceChangedConsumer), context.Message.New.Starbuild.id);
        }
    }

    private readonly ILogger<MortgageInstanceChangedConsumer> _logger;
    private readonly Database.CaseServiceDbContext _dbContext;

    public MortgageInstanceChangedConsumer(Database.CaseServiceDbContext dbContext, ILogger<MortgageInstanceChangedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
