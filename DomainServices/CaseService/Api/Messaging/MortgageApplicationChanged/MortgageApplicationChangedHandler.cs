using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class MortgageApplicationChangedHandler(
    Database.CaseServiceDbContext _dbContext, 
    ILogger<MortgageApplicationChangedHandler> _logger) 
    : IMessageHandler<cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged>
{
	public async Task Handle(IMessageContext context, cz.kb.api.mortgageservicingevents.v3.MortgageApplicationChanged message)
    {
        _logger.KafkaConsumerStarted(nameof(MortgageApplicationChangedHandler));

        if (long.TryParse(message.New.MortgageInstanceRequested.Starbuild?.id, out long caseId))
        {
            var d = (decimal)message.New.MortgageInstanceRequested.loanAmount.limit;
            int updatedRows = await _dbContext
                .Cases
                .Where(t => t.CaseId == caseId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.TargetAmount, d));

            if (updatedRows > 0)
            {
                _logger.KafkaMortgageChangedFinished(nameof(MortgageApplicationChangedHandler), caseId, d);
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