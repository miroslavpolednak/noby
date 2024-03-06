using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class MortgageApplicationChangedHandler : IMessageHandler<cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged>
{
    private readonly ILogger<MortgageApplicationChangedHandler> _logger;
    private readonly Database.CaseServiceDbContext _dbContext;

    public MortgageApplicationChangedHandler(Database.CaseServiceDbContext dbContext, ILogger<MortgageApplicationChangedHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged message)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageApplicationChangedHandler));

        if (long.TryParse(message.New.MortgageInstanceRequested.Starbuild?.id, out long caseId))
        {
            var instance = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == caseId);
            if (instance is not null)
            {
                instance.TargetAmount = (decimal)message.New.MortgageInstanceRequested.loanAmount.limit;

                await _dbContext.SaveChangesAsync();

                _logger.KafkaMortgageChangedFinished(nameof(MortgageApplicationChangedHandler), caseId, instance.TargetAmount);
            }
            else
            {
                _logger.KafkaCaseIdNotFound(nameof(MortgageApplicationChangedHandler), caseId);
            }
        }
        else
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageApplicationChangedHandler), message.New.MortgageInstanceRequested.Starbuild?.id ?? "");
        }
    }
}