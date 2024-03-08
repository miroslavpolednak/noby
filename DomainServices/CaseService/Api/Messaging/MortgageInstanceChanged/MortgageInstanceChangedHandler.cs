using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class MortgageInstanceChangedHandler : IMessageHandler<cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged>
{
    private readonly ILogger<MortgageInstanceChangedHandler> _logger;
    private readonly Database.CaseServiceDbContext _dbContext;

    public MortgageInstanceChangedHandler(Database.CaseServiceDbContext dbContext, ILogger<MortgageInstanceChangedHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged message)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageInstanceChangedHandler));
        
        if (long.TryParse(message.New.Starbuild.id, out var caseId))
        {
            var instance = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == caseId);
            if (instance is not null)
            {
                instance.TargetAmount = (decimal)message.New.loanAmount.limit;

                await _dbContext.SaveChangesAsync();

                _logger.KafkaMortgageChangedFinished(nameof(MortgageInstanceChangedHandler), caseId, instance.TargetAmount);
            }
            else
            {
                _logger.KafkaCaseIdNotFound(nameof(MortgageInstanceChangedHandler), caseId);
            }
        }
        else
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageInstanceChangedHandler), message.New.Starbuild.id);
        }
    }
}