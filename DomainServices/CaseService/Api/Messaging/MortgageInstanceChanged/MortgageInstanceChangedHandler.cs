using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class MortgageInstanceChangedHandler(
    Database.CaseServiceDbContext _dbContext, 
    ILogger<MortgageInstanceChangedHandler> _logger) 
    : IMessageHandler<cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged>
{
	public async Task Handle(IMessageContext context, cz.kb.api.mortgageservicingevents.v3.MortgageInstanceChanged message)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageInstanceChangedHandler));
        
        if (long.TryParse(message.New?.Starbuild?.id, out var caseId))
        {
            var d = (decimal)message.New.loanAmount.limit;
            int updatedRows = await _dbContext
                .Cases
                .Where(t => t.CaseId == caseId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.TargetAmount, d));

            if (updatedRows > 0)
            {
                _logger.KafkaMortgageChangedFinished(nameof(MortgageInstanceChangedHandler), caseId, d);
            }
            else
            {
                _logger.KafkaCaseIdNotFound(nameof(MortgageInstanceChangedHandler), caseId);
            }
        }
        else
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MortgageInstanceChangedHandler), message.New?.Starbuild?.id ?? string.Empty);
        }
    }
}