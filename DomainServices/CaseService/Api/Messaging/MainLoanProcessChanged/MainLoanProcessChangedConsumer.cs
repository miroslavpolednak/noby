using CIS.Foms.Enums;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Services;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MainLoanProcessChanged;

internal sealed class MainLoanProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged> context)
    {
        var message = context.Message;
        var token = context.CancellationToken;
        
        var caseState = message.state switch
        {
            cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED => CaseStates.Cancelled,
            cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED => CaseStates.Finished,
            _ => (CaseStates) message.processData.@private.mainLoanProcessData.processPhase.code
        };
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(message.@case.caseId.id);
        }

        var entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == caseId, token);

        if (entity is null)
        {
            _logger.KafkaCaseIdNotFound(caseId);
        }
        else
        {
            entity.State = (int) caseState;
        }
        
        await _dbContext.SaveChangesAsync(token);
    }
    
    private readonly CaseServiceDbContext _dbContext;
    private readonly ILogger<MainLoanProcessChangedConsumer> _logger;
    
    public MainLoanProcessChangedConsumer(
        CaseServiceDbContext dbContext,
        ILogger<MainLoanProcessChangedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
