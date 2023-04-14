using DomainServices.CaseService.Api.Database;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MainLoanProcessChanged;

internal sealed class MainLoanProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly ILogger<MainLoanProcessChangedConsumer> _logger;

    public MainLoanProcessChangedConsumer(CaseServiceDbContext dbContext, ILogger<MainLoanProcessChangedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged> context)
    {
        var state = context.Message.state switch
        {
            cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.ProcessStateEnum.TERMINATED => 7,
            cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.ProcessStateEnum.COMPLETED => 6,
            _ => context.Message.processData.@private.mainLoanProcessData.processPhase.code
        };

        if (long.TryParse(context.Message.@case.caseId.id, out long caseId))
        {
            _logger.KafkaMessageIncorrectFormat(context.Message.@case.caseId.id);
        }

        var entity = await _dbContext.Cases
            .FirstOrDefaultAsync(t => t.CaseId == caseId, context.CancellationToken);

        if (entity is null)
        {
            _logger.KafkaCaseIdNotFound(caseId);
        }

        entity!.State = state;

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
